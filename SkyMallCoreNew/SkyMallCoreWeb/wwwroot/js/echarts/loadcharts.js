//(function () {
    function loadChart() {
        $(".echarts").each(function () {
            var $chart = $(this);
            var $data = $(this).data();
            var option;
            //设置无数据的效果
            var myChart = echarts.init(this, {
                noDataLoadingOption: {
                    text: "暂无数据",
                    effect: 'bubble',
                    effectOption: {
                        backgroundColor: "#fff",
                        effect: {
                            n: 0
                        }
                    },
                    textStyle: {
                        fontSize: 20
                    }
                }
            });

            var axisX = JSON.parse($chart.attr("data-axis-x"));
            var seriesData = JSON.parse($chart.attr("data-series-data"));
            var legends = JSON.parse($chart.attr("data-legend"));

            switch ($data.type) {
                case 'bar':
                    option = (function () {
                        var o = {
                            color: ['#56b688'],
                            grid: {
                                x: 40,
                                y: 25,
                                x2: 30,
                                y2: 25,
                                borderWidth: 1
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: eval(axisX),
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: 'black'
                                        }
                                    },
                                    axisLabel: {
                                        interval: 0
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    min: 0,
                                    max: 100,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: 'black'
                                        }
                                    },
                                    axisTick: {
                                        length: 3,
                                        show: true,
                                    },
                                    splitNumber: 10
                                }
                            ],
                            series: [
                                {
                                    name: $data.name,
                                    type: 'bar',
                                    barMaxWidth: 60,
                                    itemStyle: {
                                        normal: {
                                            label: {
                                                formatter: '{c}' + '%',
                                                show: true,
                                                textStyle: {
                                                    color: 'black',
                                                    fontSize: '15',
                                                    fontFamily: '微软雅黑'
                                                }
                                            }
                                        }
                                    },
                                    data: eval($data.compositeData)
                                }
                            ]
                        };
                        return o;
                    })($data);
                    break;
                case 'muilttype':
                    option = (function () {
                        var o = {
                            color: ['#ff7f50', '#87cefa', '#da70d6', '#32cd32', '#6495ed',
                                '#ff69b4', '#ba55d3', '#cd5c5c', '#ffa500', '#40e0d0'],
                            grid: {
                                x: 40,
                                y: 25,
                                x2: 30,
                                y2: 25,
                                borderWidth: 1
                            },
                            tooltip: { show: true },
                            legend: {
                                data: legends
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    data: axisX,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: 'black'
                                        }
                                    },
                                    axisLabel: {
                                        interval: 0
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    min: 0,
                                    max: 100,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: 'black'
                                        }
                                    },
                                    axisTick: {
                                        length: 3,
                                        show: true,
                                    },
                                    splitNumber: 10
                                }
                            ],
                            series: seriesData
                        };
                        return o;
                    })($data);
                    break;
                case 'line':
                    option = (function () {
                        var o = {
                            grid: false,
                            tooltip: {
                                trigger: "item",
                                formatter: "{a}:{c}"
                            },
                            color: ['#5191d1'],
                            grid: {
                                x: 25,
                                y: 10,
                                x2: 30,
                                y2: 20,
                                borderWidth: 0
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    splitLine: { show: false },
                                    boundaryGap: true,
                                    data: eval(axisX),
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        textStyle: {
                                            color: '#5191d1'
                                        }
                                    },
                                    axisTick: {
                                        inside: true,
                                        length: 2,
                                        lineStyle: {
                                            color: '#e0e4e8',
                                            width: 1
                                        }
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    splitLine: { show: false },
                                    scale: true,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        formatter: '{value}',
                                        textStyle: {
                                            color: '#ff5f5f'
                                        }
                                    }
                                }
                            ],
                            series: [
                                {

                                    name: $data.name,
                                    type: 'line',
                                    //symbol:'none',
                                    itemStyle: {
                                        normal: {
                                            lineStyle: {
                                                width: 1,
                                                type: 'solid',
                                                color: '#5191d1'
                                            }
                                        }
                                    },
                                    data: eval($data.compositeData)
                                }
                            ]
                        };
                        return o;
                    })($data);
                    break;
                case 'twoline':
                    var _datalist = $data.compositeData.split('-');
                    var _datanamelist = $data.name.split(',');
                    option = (function () {
                        var o = {
                            legend: {
                                data: [_datanamelist[0], _datanamelist[1]],
                                selectedMode: false
                            },
                            grid: false,
                            color: ['#F47261', '#5191d1'],
                            tooltip: {
                                trigger: "axis",
                                formatter: function (params) {
                                    var _vhour = parseInt(params[0].name) - 2;
                                    var _vpara = "";
                                    var _vpara1 = "";
                                    if (params[0].value == "0" || params[0].value == "-") {
                                        _vpara = "暂无数据";
                                    }
                                    else {
                                        _vpara = params[0].value + "次";
                                    }
                                    if (params[1].value == "0" || params[1].value == "-") {
                                        _vpara1 = "暂无数据";
                                    }
                                    else {
                                        _vpara1 = params[1].value + "次";
                                    }
                                    return _vhour + "-" + params[0].name + "时<br>" + _datanamelist[0] + ":" + _vpara + "<br>" + _datanamelist[1] + ":" + _vpara1;
                                },
                                axisPointer: {
                                    type: 'line',
                                    lineStyle: {
                                        color: '#48b',
                                        width: 2,
                                        type: 'solid'
                                    },
                                    areaStyle: {
                                        size: 'auto',
                                        color: 'rgba(150,150,150,0.3)'
                                    }
                                }
                            },
                            grid: {
                                x: 30,
                                y: 20,
                                x2: 30,
                                y2: 20,
                                borderWidth: 0
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    splitLine: { show: false },
                                    boundaryGap: true,
                                    data: eval(axisX),
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        textStyle: {
                                            color: '#5191d1'
                                        }
                                    },
                                    axisTick: {
                                        inside: true,
                                        length: 2,
                                        lineStyle: {
                                            color: '#e0e4e8',
                                            width: 1
                                        }
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    splitLine: { show: false },
                                    scale: true,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        formatter: '{value}',
                                        textStyle: {
                                            color: '#ff5f5f'
                                        }
                                    }
                                }
                            ],
                            series: [
                                {
                                    name: _datanamelist[0],
                                    type: 'line',
                                    //symbol:'none',
                                    itemStyle: {
                                        normal: {
                                            lineStyle: {
                                                width: 1,
                                                type: 'solid'
                                            }
                                        }
                                    },
                                    data: eval(_datalist[0])
                                },
                                {

                                    name: _datanamelist[1],
                                    type: 'line',
                                    //symbol:'none',
                                    itemStyle: {
                                        normal: {
                                            lineStyle: {
                                                width: 1,
                                                type: 'solid'
                                            }
                                        }
                                    },
                                    data: eval(_datalist[1])
                                }
                            ]
                        };
                        return o;
                    })($data);
                    break;
                case 'pullline%':
                    option = (function () {
                        var o = {
                            grid: false,
                            tooltip: {
                                trigger: "axis",
                                formatter: function (params) {
                                    var _vhour;
                                    switch (parseInt(params[0].name)) {
                                        case 29:
                                            _vhour = parseInt(params[0].name) - 4;
                                            break;
                                        case 28:
                                        case 31:
                                            _vhour = parseInt(params[0].name) - 3;
                                            break;
                                        default:
                                            _vhour = parseInt(params[0].name) - 2;
                                            break;
                                    }
                                    if (params[0].value == "-" || params[0].value == "0") {
                                        return _vhour + "-" + params[0].name + "日<br>" + $data.name + ":暂无数据";
                                    }
                                    return _vhour + "-" + params[0].name + "日<br>" + $data.name + ":" + params[0].value + "%";
                                },
                                axisPointer: {
                                    type: 'line',
                                    lineStyle: {
                                        color: '#48b',
                                        width: 2,
                                        type: 'solid'
                                    },
                                    areaStyle: {
                                        size: 'auto',
                                        color: 'rgba(150,150,150,0.3)'
                                    }
                                }
                            },
                            color: ['#5191d1'],
                            grid: {
                                x: 50,
                                y: 15,
                                x2: 30,
                                y2: 25,
                                borderWidth: 0
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    splitLine: { show: false },
                                    boundaryGap: true,
                                    data: eval(axisX),
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        textStyle: {
                                            color: '#5191d1'
                                        }
                                    },
                                    axisTick: {
                                        inside: true,
                                        length: 2,
                                        lineStyle: {
                                            color: '#e0e4e8',
                                            width: 1
                                        }
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    splitLine: { show: false },
                                    scale: true,
                                    axisLine: {
                                        lineStyle: {
                                            width: 1,
                                            type: 'solid',
                                            color: '#e0e4e8'
                                        }
                                    },
                                    axisLabel: {
                                        formatter: '{value}' + " %",
                                        textStyle: {
                                            color: '#ff5f5f'
                                        }
                                    }
                                }
                            ],
                            series: [
                                {

                                    name: $data.name,
                                    type: 'line',
                                    //symbol:'none',
                                    itemStyle: {
                                        normal: {
                                            lineStyle: {
                                                width: 1,
                                                type: 'solid',
                                                color: '#5191d1'
                                            }
                                        }
                                    },
                                    data: eval($data.compositeData)
                                }
                            ]
                        };
                        return o;
                    })($data);
                    break;
                case 'pie':
                    option = (function () {
                        var o = {

                            series: [
                                {
                                    name: $data.name,
                                    type: 'pie',
                                    selectedMode: 'signal',
                                    itemStyle: {
                                        normal: {
                                            label: {
                                            }
                                        },
                                        emphasis: {
                                            label: {
                                                show: true,
                                                position: 'inner',
                                                formatter: "{b}\r\n{c}"
                                            }
                                        }
                                    },
                                    data: eval($data.compositeData)
                                }
                            ]
                        };
                        if ($data.labelShow == 'percent') {
                            o.series[0].itemStyle.normal.label = {
                                formatter: function (params) { return (params.percent - 0).toFixed(2) + '%'; }
                            }
                        } else {
                            if ($data.labelShow == 'textandpercent') {
                                o.series[0].itemStyle.normal.label = {
                                    formatter: function (params) {
                                        return params.name + (params.percent - 0).toFixed(2) + '%';
                                    }
                                }
                            }
                        }
                        return o;
                    })($data);
                    break;
                default:
                    break;
            }
            if (option != null) {
                if ($data.color != null) {
                    option.color = eval($data.color);
                }
                myChart.setOption(option);
            }
        });
    }
//})(echarts);
