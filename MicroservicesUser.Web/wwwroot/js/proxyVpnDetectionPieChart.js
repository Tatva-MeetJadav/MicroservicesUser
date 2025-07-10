
$(document).ready(function () {
    var rawData = $('.pie-chart-data-container').attr('data-pie-chart-data');
    var continentStats = JSON.parse(rawData);
    var labels = continentStats.map(x => x.ContinentName);
    var values = continentStats.map(x => x.Percentage);

    var data = [{
        type: 'pie',
        values: values,
        labels: labels,
        textinfo: 'label+percent',
        hole: 0.3,
        hovertemplate: '%{label} %{percent}<extra></extra>',
        marker: {
            colors: ['#007bff', '#dc3545', '#ffc107', '#17a2b8']
        }
    }];

    var layout = {
        showlegend: false,
        height: 380,
        paper_bgcolor: 'rgba(0,0,0,0)',
        plot_bgcolor: 'rgba(0,0,0,0)',
        margin: {
            l: 10,
            r: 10,
            t: 30,
            b: 10
        }
    };

    var config = {
        responsive: true,
        displaylogo: false,
        modeBarButtonsToRemove: ['toImage']
    };

    Plotly.newPlot('ipStats3DPieChart', data, layout, config);
});
