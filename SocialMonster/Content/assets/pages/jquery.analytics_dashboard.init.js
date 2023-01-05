

var options = {
    chart: {
        /*        height: 320,*/
        height: 360,
        type: 'area',
        stacked: true,
        toolbar: {
            show: false,
            autoSelected: 'zoom'
        },
    },
    colors: ['#3cba54', '#db3236', '#4885ed', ],
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'smooth',
        width: [1.5, 1.5],
/*        dashArray: [0, 4],*/
        dashArray: [0, 1],
        lineCap: 'round',
    },
    grid: {
        padding: {
            left: 0,
            right: 0
        },
        strokeDashArray: 3,
    },
    markers: {
        size: 0,
        hover: {
            size: 0
        }
    },
    series: [{
        name: 'Twitter',
        data: twitterChartCount1 ,
    }, {
        name: 'Facebook',
        data: facebookChartCount1,
        },
        {
            name: 'Website',
            data: websiteChartCount1,
        }],

    xaxis: {
        type: 'month',
        categories: [30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: true,
        },
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.4,
            opacityTo: 0.3,
            stops: [0, 90, 100]
        }
    },

    tooltip: {
        x: {
            format: 'dd/MM/yy HH:mm'
        },
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right'
    },
}

var chart = new ApexCharts(
    document.querySelector("#ana_dash_1"),
    options
);

chart.render();

//bulgaa 2022-04-27
var options = {
    chart: {
        height: 180,
        type: 'area',
        stacked: false,
        toolbar: {
            show: false,
            autoSelected: 'zoom'
        },
    },
    colors: ['#3B5998', '#1DA1F2', '#969bd6',],
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'smooth',
        width: [1.5, 1.5],
        dashArray: [0, 3],
        lineCap: 'round',
    },
    grid: {
        padding: {
            left: 0,
            right: 0
        },
        strokeDashArray: 1,
    },
    markers: {
        size: 0,
        hover: {
            size: 0
        }
    },
    series: [{
        name: 'Twitter',
        data: twitterChartCount1,
    }, {
        name: 'Facebook',
        data: facebookChartCount1,
    },
    {
        name: 'Website',
        data: websiteChartCount1,
    }],

    xaxis: {
        type: 'month',
        categories: [30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: true,
        },
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.4,
            opacityTo: 0.3,
            stops: [0, 90, 100]
        }
    },

    tooltip: {
        x: {
            format: 'dd/MM/yy HH:mm'
        },
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right'
    },
}

var chart = new ApexCharts(
    document.querySelector("#ana_dash_2"),
    options
);

chart.render();

// Spark


// var options = {
//   series: [76],
//   chart: {
//   type: 'radialBar',
//   offsetY: -20,
//   sparkline: {
//     enabled: true
//   }
// },
// plotOptions: {
//   radialBar: {
//     startAngle: -90,
//     endAngle: 90,  
//     hollow: {
//       size: '75%',
//       position: 'front',
//   },  
//     track: {
//       background: ["rgba(42, 118, 244, .18)"],
//       strokeWidth: '80%',
//       opacity: 0.5,
//       margin: 5,
//     },
//     dataLabels: {
//       name: {
//         show: false
//       },
//       value: {
//         offsetY: -2,
//         fontSize: '20px'
//       }
//     }
//   }
// },
// stroke: {
//   lineCap: 'butt'
// },
// colors: ["#2a76f4"],
// grid: {
//   padding: {
//     top: -10
//   }
// },

// labels: ['Average Results'],
// };

// var chart = new ApexCharts(document.querySelector("#ana_1"), options);
// chart.render();



//Device-widget

 
var options = {
  chart: {
      height: 270,
      type: 'donut',
  }, 
  plotOptions: {
    pie: {
      donut: {
        size: '85%'
      }
    }
  },
  dataLabels: {
    enabled: false,
  },

  stroke: {
    show: true,
    width: 2,
    colors: ['transparent']
  },
 
  series: [50, 25, 25,],
  legend: {
    show: true,
    position: 'bottom',
    horizontalAlign: 'center',
    verticalAlign: 'middle',
    floating: false,
    fontSize: '13px',
    offsetX: 0,
    offsetY: 0,
  },
  labels: [ "Mobile1","Tablet", "Desktop" ],
  colors: ["#2a76f4","rgba(42, 118, 244, .5)","rgba(42, 118, 244, .18)"],
 
  responsive: [{
      breakpoint: 600,
      options: {
        plotOptions: {
            donut: {
              customScale: 0.2
            }
          },        
          chart: {
              height: 240
          },
          legend: {
              show: false
          },
      }
  }],
  tooltip: {
    y: {
        formatter: function (val) {
            return   val + " %"
        }
    }
  }
  
}

var chart = new ApexCharts(
  document.querySelector("#ana_device"),
  options
);

chart.render();




var colors = ['#98e7df', '#b8c4d0', '#bec7fa', '#ffe2a3', '#92e6f0'];
var options = {
  series: [{
  name: 'Inflation',
  data: [ 4.0, 10.1, 6.0, 8.0, 9.1]
}],
  chart: {
  height: 355,
  type: 'bar',
  toolbar:{
    show:false
  },
},
plotOptions: {
  bar: {
      dataLabels: {
          position: 'top', // top, center, bottom              
      },
      columnWidth: '20',
      distributed: true,
  },

},
dataLabels: {
  enabled: true,
  formatter: function (val) {
    return val + "%";
  },
  offsetY: -20,
  style: {
    fontSize: '12px',
    colors: ["#000"]
  }
},
colors: colors,
xaxis: {
  categories: ["Email", "Referral", "Organic", "Direct", "Campaign",],
  position: 'top',
  axisBorder: {
    show: false
  },
  axisTicks: {
    show: false
  },
  crosshairs: {
    fill: {
      type: 'gradient',
      gradient: {
        colorFrom: '#D8E3F0',
        colorTo: '#BED1E6',
        stops: [0, 100],
        opacityFrom: 0.4,
        opacityTo: 0.5,
      }
    }
  },
  tooltip: {
    enabled: true,
  },
},

grid: {
  padding: {
    left: 0,
    right: 0
  },
  strokeDashArray: 3,
},
yaxis: {
  axisBorder: {
    show: false
  },
  axisTicks: {
    show: false,
  },
  labels: {
    show: false,
    formatter: function (val) {
      return val + "%";
    }
  }

},
};

var chart = new ApexCharts(document.querySelector("#barchart"), options);
chart.render();


//2022-05-11
var options = {
    chart: {
        /*        height: 320,*/
        height: 200,
        type: 'area',
        stacked: true,
        toolbar: {
            show: false,
            autoSelected: 'zoom'
        },
    },
    colors: ['#008dcd', '#0071a4', '#525252',],
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'smooth',
        width: [1.5, 1.5],
        /*        dashArray: [0, 4],*/
        dashArray: [0, 1],
        lineCap: 'round',
    },
    grid: {
        padding: {
            left: 0,
            right: 0
        },
        strokeDashArray: 3,
    },
    markers: {
        size: 0,
        hover: {
            size: 0
        }
    },
    series: [{
        name: 'Positive',
        data: TwPosChartCount,
    }, {
        name: 'Negative',
        data: TwNegChartCount,
    },
    {
        name: 'Neutral',
        data: TwNeuChartCount,
    }],

    xaxis: {
        type: 'month',
        categories: [30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: true,
        },
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.4,
            opacityTo: 0.3,
            stops: [0, 90, 100]
        }
    },

    tooltip: {
        x: {
            format: 'dd/MM/yy HH:mm'
        },
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right'
    },
}

var chart = new ApexCharts(
    document.querySelector("#TwitterAnalyze"),
    options
);

chart.render();

var options = {
    chart: {
        /*        height: 320,*/
        height: 200,
        type: 'area',
        stacked: true,
        toolbar: {
            show: false,
            autoSelected: 'zoom'
        },
    },
    colors: ['#3b5998', '#8b9dc3', '#dfe3ee',],
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'smooth',
        width: [1.5, 1.5],
        /*        dashArray: [0, 4],*/
        dashArray: [0, 1],
        lineCap: 'round',
    },
    grid: {
        padding: {
            left: 0,
            right: 0
        },
        strokeDashArray: 3,
    },
    markers: {
        size: 0,
        hover: {
            size: 0
        }
    },
    series: [{
        name: 'Positive',
        data: FbPosChartCount,
    }, {
        name: 'Negative',
        data: FbNegChartCount,
    },
    {
        name: 'Neutral',
        data: FbNeuChartCount,
    }],

    xaxis: {
        type: 'month',
        categories: [30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: true,
        },
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.4,
            opacityTo: 0.3,
            stops: [0, 90, 100]
        }
    },

    tooltip: {
        x: {
            format: 'dd/MM/yy HH:mm'
        },
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right'
    },
}

var chart = new ApexCharts(
    document.querySelector("#FacebookAnalyze"),
    options
);

chart.render();

var options = {
    chart: {
        /*        height: 320,*/
        height: 200,
        type: 'area',
        stacked: false,
        toolbar: {
            show: false,
            autoSelected: 'zoom'
        },
    },
    colors: ['#920e84', '#542cfa', '#23e4ff',],
    dataLabels: {
        enabled: false
    },
    stroke: {
        curve: 'smooth',
        width: [1.5, 1.5],
        /*        dashArray: [0, 4],*/
        dashArray: [0, 1],
        lineCap: 'round',
    },
    grid: {
        padding: {
            left: 0,
            right: 0
        },
        strokeDashArray: 3,
    },
    markers: {
        size: 0,
        hover: {
            size: 0
        }
    },
    series: [{
        name: 'Positive',
        data: WebPosChartCount,
    }, {
        name: 'Negative',
        data: WebNegChartCount,
    },
    {
        name: 'Neutral',
        data: WebNeuChartCount,
    }],

    xaxis: {
        type: 'month',
        categories: [30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: true,
        },
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 1,
            opacityFrom: 0.4,
            opacityTo: 0.3,
            stops: [0, 90, 100]
        }
    },

    tooltip: {
        x: {
            format: 'dd/MM/yy HH:mm'
        },
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right'
    },
}

var chart = new ApexCharts(
    document.querySelector("#WebsiteAnalyze"),
    options
);

chart.render();
//bulgaa 2022-05-11