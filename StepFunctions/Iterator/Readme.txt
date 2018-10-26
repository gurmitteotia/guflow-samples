You can find iterator example in StepFunctions at https://docs.aws.amazon.com/step-functions/latest/dg/tutorial-create-iterate-pattern-section.html

In Guflow example example you can handle is much simple way as shown in this example. Unlike StepFunctions you do not need a seperate lambda function to maintain the count.
Guflow itself can figure out the total number a times a function is executed.
