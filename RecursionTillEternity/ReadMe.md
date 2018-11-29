This example shows how you can recursively execute the "ProcessLogWorkflow" till eternity and that too efficiently.

In Guflow you can recursively execute the workflow using two approches:
 * Implement the recursion in same workflow as show in [this](https://github.com/gurmitteotia/guflow-samples/tree/master/LoopSupport) example.
 * Split the "recursive" logic in to seperate workflow as is being done in this example.

  
 In this example we have implement the second approach and it has following extra benefits over first approach:
  * More efficient, "DriverWorkflow", where recursive logic lives, uses custom polling strategy to download only one page (maximum 1000) of history events. It does not need to download 
    entire history events (which is roughly 25000 at peak) to make recursive logic work.
  * Better control on error handling. If for any reason "ProcessLogWorkflow" is terminated, timedout or failed "DriverWorkflow" will immediately reschedule it.

  Please note that custom polling strategy brings some [restrictions](https://github.com/gurmitteotia/guflow/wiki/Custom-polling-strategy)
