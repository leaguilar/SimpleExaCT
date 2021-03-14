FROM jupyter/scipy-notebook

# Modified from https://stackoverflow.com/questions/59548454/python3-mayavi-in-docker-not-installing
# source: https://github.com/FelipeLema/mne-binder/blob/master/Dockerfile
# source mantainer: MAINTAINER Jean-Remi King <jeanremi.king@gmail.com>
# source BSD Licence: https://github.com/FelipeLema/mne-binder/blob/master/LICENSE

# Install core debian packages
USER root
ENV DEBIAN_FRONTEND noninteractive
RUN apt-get update && apt-get -yq dist-upgrade \
    && apt-get install -yq --no-install-recommends \
    openssh-client \
    vim \
    curl \
    gcc \
    && apt-get clean

# Xvfb
RUN apt-get install -yq --no-install-recommends \
    xvfb \
    x11-utils \
    libx11-dev \
    qt5-default \
    && apt-get clean

ENV DISPLAY=:99

# Switch to notebook user
USER $NB_UID

# Upgrade the package managers
RUN pip install --upgrade pip
RUN npm i npm@latest -g

# Install Python packages
RUN pip install vtk && \
    pip install boto && \
    pip install h5py && \
    pip install nose && \
    pip install ipyevents && \
    pip install ipywidgets && \
    pip install mayavi && \
    pip install nibabel && \
    pip install numpy && \
    pip install pillow && \
    pip install pyqt5 && \
    pip install scikit-learn && \
    pip install scipy && \
    pip install xvfbwrapper && \
    pip install https://github.com/nipy/PySurfer/archive/master.zip

# Install Jupyter notebook extensions
RUN pip install RISE && \
    jupyter nbextension install rise --py --sys-prefix && \
    jupyter nbextension enable rise --py --sys-prefix && \
    jupyter nbextension install mayavi --py --sys-prefix && \
    jupyter nbextension enable mayavi --py --sys-prefix && \
    npm cache clean --force

# Try to decrease initial IPython kernel load times
RUN ipython -c "import matplotlib.pyplot as plt; print(plt)"

COPY . .
RUN pip install -r requirements.txt

# Add an x-server to the entrypoint. This is needed by Mayavi
ENTRYPOINT ["tini", "-g", "--", "xvfb-run"]
