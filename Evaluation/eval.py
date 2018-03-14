import pandas as pd
import numpy as np


def evaluate_preds(a, p):
    cm = pd.DataFrame({'actual': a, 'predicted': p})
    cm = pd.crosstab(cm['actual'], cm['predicted'])
    labels = cm.columns
    cm = cm.as_matrix()
    col_sums = np.sum(cm, axis=0)
    row_sums = np.sum(cm, axis=1)
    diag = cm.diagonal()
    n = sum(sum(cm))
    accuracy = sum(diag) / n
    precision = diag / col_sums
    recall = diag / row_sums
    f1 = 2 * precision * recall / (precision + recall)
    macro_precision = precision.mean()
    macro_recall = recall.mean()
    macro_f1 = f1.mean()

    return ({'n': n,
             'accuracy': accuracy,
             'precision': precision.tolist(),
             'recall': recall.tolist(),
             'f1': f1.tolist(),
             'macro_precision': macro_precision,
             'macro_recall': macro_recall,
             'macro_f1': macro_f1,
             'cm': cm,
             'labels': labels.tolist()})

