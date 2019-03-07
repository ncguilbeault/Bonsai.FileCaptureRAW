# Bonsai.FileCaptureRAW
[Bonsai](https://bonsai-rx.org/) library module for reading videos with the RAW video format.

## Installation
Download or clone the repository. From within Bonsai, open up the Bonsai Package Manager, go to settings, and then add the .nuget folder path found in the Bonsai.FileCaptureRAW repository to the list of available package sources.
After adding the folder path to the list of available package sources, look for the Bonsai.FileCaptureRAW package in online packages and click to install the package.

## Usage
After installing the package, a **FileCaptureRAW** source node will become available to add to your workflows. 
If everything has been installed correctly, you will now be able to read from videos with the RAW video format.

### Notes:
I originally developed this package because I was getting an error with original FileCapture node when trying to read .avi videos with a RAW data format. It circumvents the read/write protected memory error with the original FileCapture node.
