Charts
======

This is a graph of fake user data using [d3js].  

The top graph contains an area (blue) representing the total number of users and an area (maroon) of the percentage of users who are having connection issues graphed over time.

The bottom graph shows the same user count data, but compressed and allowing the user to select by dragging a subset of the total time to be displayed in the upper graph.

Based on [Focus+Context via Brushing]
[d3js]: http://d3js.org
[Focus+Context via Brushing]: http://bl.ocks.org/1667367
##Data Format
To use this, your data must be in the TSV (Tab Separated Values) format with the following headers:

```
date	total	sessions	fallback	ratio
```

D3js then uses [d3.tsv] to parse the TSV and renders it using [d3.svg.area].
Also, the date must be in the format ```2012-12-05T17:00:00.0000000-08:00```
which is ```yyyy-MM-ddTHH:mm:ss.0000000-08:00``` because then it matches the data returned by MDS.

[d3.tsv]:https://github.com/mbostock/d3/wiki/CSV
[d3.svg.area]:https://github.com/mbostock/d3/wiki/SVG-Shapes#wiki-area
