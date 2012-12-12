var log = function (s) { $('#Log').append('<li>' + s + '</li>'); }
var parseDate = d3.time.format("%Y-%m-%dT%H:%M").parse;

var margin = { top: 10, right: 30, bottom: 100, left: 110 },
    margin2 = { top: 540, right: 30, bottom: 0, left: 110 },
    width = $(document).width() - margin.left - margin.right,
    height = 600 - margin.top - margin.bottom,
    height2 = 600 - margin2.top - margin2.bottom;

var xAxisValue = function (d) { return x(d.date); };


// total players
var x = d3.time.scale().range([0, width]);
var x2 = d3.time.scale().range([0, width]);

var y = d3.scale.linear().range([height, 0]);
var yAxis = d3.svg.axis().scale(y).orient("left");
var line = d3.svg.line()
    .x(xAxisValue)
    .y(function (d) { return y(d.total); });

// Time
var xAxis = d3.svg.axis().scale(x).orient("bottom");

// % of fallback to ok users
var y2 = d3.scale.linear().range([height, 0]);
var yAxis2 = d3.svg.axis().scale(y2).orient('right');
var ratioLine = d3.svg.line()
    .x(xAxisValue)
    .y(function (d) { return y2(d.ratio); });

// Bottom view
var y3 = d3.scale.linear().range([height2, 0]);
var yAxis3 = d3.svg.axis().scale(y3).orient('left');

var bottomAsLine = d3.svg.line()
    .x(function (d) { return x2(d.date); })
    .y(function (d) { return y3(d.total); });


var bottomAsArea = d3.svg.area()
    .interpolate("monotone")
    .x(function (d) { return x2(d.date); })
    .y0(height2)
    .y1(function (d) { return y3(d.total); });

var prepData = function (error, data) {
    data.forEach(function (d) {
        try {
            d.date = parseDate(d.date.substr(0, 16));
            d.total = +d.total;
            d.ratio = +d.ratio;
            d.fallback = +d.fallback;
        }
        catch (err) { log(err); }
    });
    x.domain(d3.extent(data, function (d) { return d.date; }));
    y.domain(d3.extent(data, function (d) { return d.total; }));

    // better w/o mapping to the min & max.
    //y2.domain(d3.extent(data, function (d) { return d.ratio; }));

    y3.domain(d3.extent(data, function (d) { return d.total; }));

    x2.domain(x.domain());
 
    var svg = d3.select("body").append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom);
    var main = svg.append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    main.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);

    main.append("defs").append("clipPath")
        .attr("id", "clip")
      .append("rect")
        .attr("width", width)
        .attr("height", height);

    main.append("g")
        .attr("class", "y axis axis1")
        .call(yAxis)
      .append("text")
        .attr("x", -6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text("Users")
        .attr('class', 'label1');

    main.append("g")
       .attr("class", "y axis 2 axis2")
       .call(yAxis2)
     .append("text")
       .attr("x", -6)
       .attr("y", height - 15)
       .attr("dy", ".71em")
       .style("text-anchor", "end")
       .text("fallback %")
        .attr('class', 'label2');

    main.append("path")
        .datum(data)
        .attr("class", "line")
        .attr("d", line);

    main.append("path")
       .datum(data)
       .attr("class", "line2")
       .attr("d", ratioLine);

    var context = svg.append("g")
       .attr("transform", "translate(" + margin2.left + "," + margin2.top + ")");

    // draw the line
    context.append("path")
     .datum(data)
     .attr("class", "line3")
     .attr("d", bottomAsArea);

    context.append("g")
    .attr("class", "x axis")
    .attr("transform", "translate(0," + height2 + ")")
    .call(xAxis);

    var brush = d3.svg.brush()
        .x(x2)
        .on("brush", brush);

    function brush() {
        var newDomain = brush.empty() ? x.domain() : brush.extent();
        x.domain(newDomain);
        log(newDomain);
        main.selectAll("path.line").attr("d", line);
        main.selectAll("path.line2").attr("d", ratioLine);
        main.select(".x.axis").call(xAxis);
    }

    context.append("g")
      .attr("class", "x brush")
      .call(brush)
    .selectAll("rect")
      .attr("y", -6)
      .attr("height", height2 + 7);
}