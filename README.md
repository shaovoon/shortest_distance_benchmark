# shortest_distance_benchmark
Benchmark of finding the shortest distance with sqrt or without sqrt

Built in Release x64.

```
VC++ 2019 16.4 update, /Ox
============================
   With sqrt timing: 4855ms
Without sqrt timing: 1264ms
	
Clang++ 6.0.0, -O3
============================
   With sqrt timing: 1830ms
Without sqrt timing: 1047ms

G++ 7.4.0, -O3
============================
   With sqrt timing: 2388ms
Without sqrt timing: 1211ms

C# 7, .Net Framework 4.6.1
============================
   With sqrt timing: 1938ms
Without sqrt timing: 1840ms
```
