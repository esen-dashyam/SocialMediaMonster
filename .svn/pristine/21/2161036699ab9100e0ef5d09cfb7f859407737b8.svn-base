

var chart1_option = {
    series: media,
    labels: ['Вэбсайт', 'Телевиз', 'Сонин, сэтгүүл'],
    colors: ['#8067dc', '#6771dc', '#6794dc', '#E91E63'],
    chart: {
        type: 'donut',
    },
    plotOptions: {
        pie: {
          donut: {
            // size: '70%'
          }
        }
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart1 = new ApexCharts(document.querySelector("#chart1"), chart1_option);
chart1.render();


// alert(facebook_count);
var chart2_option = {
    series: social,
    labels: ['Facebook', 'Twitter', 'Instagram', 'LinkedIn', 'Youtube'],
    colors: ['#1778F2', '#1DA1F2', '#6771dc', '#6794dc', '#8067dc'],
    chart: {
        type: 'donut',
    },
    stroke: {
        show: false,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500,
            foreColor: '#000'
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart2 = new ApexCharts(document.querySelector("#chart2"), chart2_option);
chart2.render();

var chart3_option = {
    series: sentiment,
    labels: ['Эерэг', 'Сөрөг', 'Саармаг'],
    colors: ['#0fb36c', '#f55', '#94baf9'],
    chart: {
        type: 'donut',
    },
    stroke: {
        show: false,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500,
            foreColor: '#000'
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart3 = new ApexCharts(document.querySelector("#chart3"), chart3_option);
chart3.render();

var chart4_option = {
    series: social_x_traditional,
    labels: ['Нийгмийн сүлжээ', 'ХМХэрэгсэл'],
    colors: ['#6771dc', '#6794dc'],
    chart: {
        type: 'donut',
    },
    stroke: {
        show: false,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500,
            foreColor: '#000'
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart4 = new ApexCharts(document.querySelector("#chart4"), chart4_option);
chart4.render();

var chart5_option = {
    series: fb_sentiment,
    labels: ['Эерэг', 'Сөрөг', 'Саармаг'],
    colors: ['#0fb36c', '#f55', '#94baf9'],
    chart: {
        type: 'donut',
    },
    stroke: {
        show: false,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500,
            foreColor: '#000'
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart5 = new ApexCharts(document.querySelector("#chart5"), chart5_option);
chart5.render();

var chart6_option = {
    series: tw_sentiment,
    labels: ['Эерэг', 'Сөрөг', 'Саармаг'],
    colors: ['#0fb36c', '#f55', '#94baf9'],
    chart: {
        type: 'donut',
    },
    stroke: {
        show: false,
        width: 2,
        colors: ['transparent']
    },
    dataLabels: {
        enabled: true,
        dropShadow: {
            enabled: false
        },
        style: {
            fontSize: '12px',
            fontFamily: 'Roboto',
            fontWeight: 500,
            foreColor: '#000'
        },
    },
    legend: {
        show: true,
        position: 'bottom',
    },
    responsive: [{
        breakpoint: 600,
        options: {
            chart:{
                    // height: 240
                },
        },
    }]
};
var chart6 = new ApexCharts(document.querySelector("#chart6"), chart6_option);
chart6.render();

var chart_weekly_options = {
    chart: {
        height: 296,
        type: 'bar',
        toolbar: {
            show: false
        },
    },
    plotOptions: {
        bar: {
            horizontal: false,
            // endingShape: 'rounded',
            columnWidth: '80%',
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    colors: ["#8067dc", '#6771dc', "#6794dc"],
    series: [{
        name: 'Вэбсайт',
        data: [44, 55, 57, 56, 61, 58, 63]
    }, {
        name: 'Телевиз',
        data: [6, 8, 11, 8, 7, 10, 1]
    }, {
        name: 'Сонин, сэтгүүл',
        data: [5, 4, 6, 6, 5, 4, 5]
    }],
    xaxis: {
        categories: ['7 хон', '6 хон', '5 хон', '4 хон', '3 хон', '2 хон', '1 хон'],
        axisBorder: {
            show: true,
            color: '#bec7e0',
          },  
          axisTicks: {
            show: true,
            color: '#bec7e0',
        },    
    },
    legend: {
        offsetY: 6,
    },
    // yaxis: {
    //     title: {
    //         text: '$ (thousands)'
    //     }
    // },
    fill: {
        opacity: 1
  
    },
    grid: {
        row: {
            colors: ['transparent', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.2
        },
        borderColor: '#f1f3fa'
    },
    tooltip: {
        y: {
            formatter: function (val) {
                return val
            }
        }
    }
}
var chart_weekly = new ApexCharts(document.querySelector("#chart_weekly"), chart_weekly_options);
chart_weekly.render();

$(document).ready(function() {
    $.get({
        url: last_months_url,
        dataType: 'json',
        success: function (data){
            $('#chart_last_months').html(data.view);
        }
    })
})

$(document).ready(function() {
    $.get({
        url: latest_data_url,
        dataType: 'json',
        success: function (data) {
            var chart_month_options = {
                colors: ["#8067dc", '#6771dc'],
                series: [{
                    name: 'Facebook',
                    data: data.fb_days
                }, {
                    name: 'Twitter',
                    data: data.tw_days
                }],
                chart: {
                    height: 350,
                    type: 'area',
                    toolbar: {
                        show: false
                    },
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'smooth',
                    width: 1.5,
                    lineCap: 'round',
                },
                grid: {
                    padding: {
                      left: 0,
                      right: 0
                    },
                    strokeDashArray: 3,
                },
                xaxis: {
                    type: 'datetime',
                    categories: data.days
                },
                tooltip: {
                    x: {
                        format: 'yyyy-MM-dd'
                    },
                },
                legend: {
                    position: 'top',
                    horizontalAlign: 'right'
                },
            };
            var chart_month = new ApexCharts(document.querySelector("#chart_month"), chart_month_options);
            chart_month.render();
        },
    });
});

function date_range(){
    $.post({
        url: date_url,
        dataType: 'json',
        data: {fromDate: $('#fromDate').val(), toDate: $('#toDate').val(), object_id: object_id, _token : token},
        success: function (data) {
            $('#chart1').html("");
            $('#chart2').html("");
            $('#chart4').html("");
            $('#chart3').html("");
            $('#chart5').html("");
            $('#chart6').html("");
            $('#chart8').html("");
            $('#chart1_table').html("");
            $('#chart2_table').html("");
            $('#chart4_table').html("");
            $('#chart3_table').html("");
            $('#chart5_table').html("");
            $('#chart6_table').html("");
            chart1_option.series = [data.info.web_count, data.info.television_count, data.info.newspaper_count];
            var chart1 = new ApexCharts(document.querySelector("#chart1"), chart1_option);
            chart1.render();
            chart2_option.series = [data.info.facebook_count, data.info.twitter_count, data.info.instagram_count, data.info.linkedin_count, data.info.youtube_count];
            var chart2 = new ApexCharts(document.querySelector("#chart2"), chart2_option);
            chart2.render();
            chart3_option.series = [data.info.pos_count, data.info.neg_count, data.info.neu_count];
            var chart3 = new ApexCharts(document.querySelector("#chart3"), chart3_option);
            chart3.render();
            chart4_option.series = 
            [
                data.info.facebook_count + data.info.twitter_count + data.info.instagram_count + data.info.youtube_count,
                data.info.web_count + data.info.television_count + data.info.newspaper_count
            ];
            var chart4 = new ApexCharts(document.querySelector("#chart4"), chart4_option);
            chart4.render();

            chart5_option.series = [data.info.fb_sentiment.pos, data.info.fb_sentiment.neg, data.info.fb_sentiment.neu];
            var chart5 = new ApexCharts(document.querySelector("#chart5"), chart5_option);
            chart5.render();

            chart6_option.series = [data.info.tw_sentiment.pos, data.info.tw_sentiment.neg, data.info.tw_sentiment.neu];
            var chart6 = new ApexCharts(document.querySelector("#chart6"), chart6_option);
            chart6.render();
        },

    });
}