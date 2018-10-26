In this case when ParentWorkflow starts it schedules the KidPlayWorkflow as its child workflow.
KidPlayWorkflow schedule the following activity in sequence and they are cancellable:
* PlayOnSwing
* PlayOnZipWire

This example further shows how ParentWorkflow and KidPlayWorkflow communicate with each other using signals.
ParentWorkflow also cancel the KidPlayWorkflow collaboratively by sending the signals.

