import numpy as np
from scipy import stats

class KDE:
    def __init__(self,xyz):
        self.kde = stats.gaussian_kde(xyz) 
    def calc_kde(self,data):
        return self.kde(data.T)
