# UXC.Devices.EyeTracker

## Calibration

### Calibrator states
* None - the calibrator was initialized, does nothing,
* Preparation - the calibrator is preparing for calibration, i.e., requests user to position himself, 
* Running - performs the calibration,
* Finished - the calibration process completed,
* Completed - the calibration was submitted, presumably by the user,
* Canceled - the calibration was cancelled.