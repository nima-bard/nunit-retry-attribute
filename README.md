# nunit-retry
A NUnit retry attribute for applying retry command in assembly level.

Simply update Assembly.cs file of your NUnit test project to enable the retry mechanism on test errors and failures:

```
  [assembly: RetryOnError(5)]
```

This attribute can also be applied on test fixtures, test method and test cases. 

You can use NoRetryAttribute to ignore the retry command over a specific test fixture or method.
