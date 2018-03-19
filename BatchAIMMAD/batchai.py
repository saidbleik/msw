from azure.common.credentials import ServicePrincipalCredentials
from azure.mgmt.compute import ComputeManagementClient
import azure.mgmt.batchai as batchai
import azure.mgmt.batchai.models as baimodels
import os

# Azure service principle login credentials
TENANT_ID = ''
CLIENT = ''
KEY = ''

# Batch AI cluster info
resource_group_name = ''
subscription_id = ''
cluster_name = ''
location = 'eastus'
command_line = 'python /mnt/batch/tasks/shared/LS_root/mounts/bfs/train.py {0} {1} {2} {3}'
std_out_err_path_prefix = '/mnt/batch/tasks/shared/LS_root/mounts/bfs'
node_count = 2

# job parameters
ts_from = '2018-03-08'
ts_to = '2018-03-10'
device_ids = [1, 2, 3]
tags = [1, 2, 3, 4, 5]

credentials = ServicePrincipalCredentials(
    client_id=CLIENT,
    secret=KEY,
    tenant=TENANT_ID
)

batchai_client = batchai.BatchAIManagementClient(
    credentials=credentials, subscription_id=subscription_id)
cluster = batchai_client.clusters.get(resource_group_name, cluster_name)

# run an async job for each sensor
for device_id in device_ids:
    for tag in tags:
        job_name = 'train{0}_{1}'.format(device_id, tag)
        custom_settings = baimodels.CustomToolkitSettings(
            command_line=command_line.format(device_id, tag, ts_from, ts_to))
        print('command line: ' + custom_settings.command_line)
        params = baimodels.job_create_parameters.JobCreateParameters(location=location,
                                                                     cluster=baimodels.ResourceId(
                                                                         cluster.id),
                                                                     node_count=node_count,
                                                                     std_out_err_path_prefix=std_out_err_path_prefix,
                                                                     custom_toolkit_settings=custom_settings
                                                                     )

        batchai_client.jobs.create(resource_group_name, job_name, params)


# print or delete jobs
#for j in batchai_client.jobs.list():
    #print(j.name)
    #batchai_client.jobs.delete(resource_group_name, j.name)
