This example shows how you can recursively execute the "ProcessLogWorkflow" till eternity and that too efficiently.

In Guflow you can recursively execute the workflow using two approches:
 * Implement the recrusion in same workflow as show in [this](this) example.
 * Split the "recursive" logic in to seperate workflow as is being done in this example.

 Both are recommened approaches and it entirely depends on your requirements 
 
 Second approach add some benefits to first approach:
  * More efficient, "DriverWorkflow" where recursive logic lives uses custom polling strategy to download only first page of history events. It does not neeed entire history
    to be downloaded to make recursive logic work.
  * 

