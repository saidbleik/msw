AutoFeaturize = function(dataset, featcols, groupby, aggfuns) 
{
  featcols = unlist(strsplit(gsub(" ","",featcols),split = ","))
  aggfuns = unlist(strsplit(gsub(" ","",aggfuns),split = ","))
  groupby = unlist(strsplit(gsub(" ","",groupby),split = ","))
 
  library(data.table)
  dt = data.table(dataset)
  setkeyv(dt, groupby)
  
  res = sapply(aggfuns,
               function(f){
                 sapply(featcols,
                        function(c){
                          t=dt[,list(do.call(f,list(get(c)))), by=groupby]                         
                          t[,!seq_along(groupby), with=F]
                        },
                        simplify=F, USE.NAMES = T)
               },
               simplify=F, USE.NAMES = T
  )
  
  res_groupcolumns=dt[,length(.SD), by=groupby]
  res = cbind(res_groupcolumns[,seq_along(groupby), with=F], as.data.frame(res))
  setnames(res, colnames(res), c(groupby , as.vector(t(sapply(featcols, paste, aggfuns, sep="_")))))
  return(as.data.frame(res))
}