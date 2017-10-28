(function getFundPerformance() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "http://localhost:3998/handlers/FundPerformanceService.svc/GetFundPerformance",
        datatype: "json",
        success: function (result) {
            parceFunds(JSON.parse(result.d));
            //console.log(result.d);
        },
        error: function (x, e) {
            if (x.status == 0) {
                alert('You are offline!!\n Please Check Your Network.');
            } else if (x.status == 404) {
                alert('Requested URL not found.');
            } else if (x.status == 500) {
                alert('Internal Server Error.');
            } else if (e == 'parsererror') {
                alert('Error.\nParsing JSON Request failed.');
            } else if (e == 'timeout') {
                alert('Request Time out.');
            } else {
                alert('Unknow Error.\n' + x.responseText);
            }
        }
    });
})();

function parceFunds(data) {

    $(data.Performances).each(function () {
        dataSource.push({
            PerformanceDate: new Date(parseInt(this.PerformanceDate.replace('/Date(', ''))).toISOString().slice(0, 10),
            MonthlyReturn: (this.MonthlyReturn * 100),
            TotalReturn: (this.TotalReturn * 100) + " %"
        });

        $("#fundPerformance").html($("#fundPerformance").html() + "<tr><td>" + new Date(parseInt(this.PerformanceDate.replace('/Date(', ''))).toISOString().slice(0, 10) + "</td><td>" + (this.MonthlyReturn * 100) + " %</td><td>" + this.TotalReturn + "</td>");
    });

    drawChart();
}

function drawADxLineChart() {

}

var dataSource = [];

var types = ["line", "stackedline", "fullstackedline"];

// chart

function drawChart() {
    console.log(dataSource);
    var chart = $("#chart").dxChart({
        palette: "violet",
        dataSource: dataSource,
        commonSeriesSettings: {
            argumentField: "PerformanceDate",
            type: types[0]
        },
        margin: {
            bottom: 20
        },
        argumentAxis: {
            valueMarginsEnabled: false,
            discreteAxisDivisionMode: "crossLabels",
            grid: {
                visible: true
            }
        },
        series: [
            { valueField: "TotalReturn", name: "Total Return" }
        ],
        legend: {
            verticalAlignment: "bottom",
            horizontalAlignment: "center",
            itemTextPosition: "bottom"
        },
        title: {
            text: "Fund Performance",
            subtitle: {
                text: "Total return"
            }
        },
        "export": {
            enabled: true
        },
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText
                };
            }
        }
    }).dxChart("instance");

    $("#types").dxSelectBox({
        dataSource: types,
        value: types[0],
        onValueChanged: function (e) {
            chart.option("commonSeriesSettings.type", e.value);
        }
    });
}
