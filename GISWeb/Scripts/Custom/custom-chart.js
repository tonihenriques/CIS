
function TotalIncidentesChartBar(nomeDiv) {

    $("#" + nomeDiv).css("width", $("#" + nomeDiv).parent().width() + "px");

    google.charts.load('current', { packages: ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(function () { LoadTotalIncidentesChartBar(nomeDiv); });
}

function LoadTotalIncidentesChartBar(nomeDiv) {

    $.ajax({
        method: "POST",
        url: "/Home/LoadIncidentesBar",
        data: {  },
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            TratarResultadoJSON(content.resultado);

            if (content.resultado.Conteudo.length > 0) {

                var data = google.visualization.arrayToDataTable(eval("[" + content.resultado.Conteudo + "]"));

                var options = {
                    title: 'Total Incidentes por Mês',
                    chartArea: {
                        backgroundColor: 'transparent',
                        left: 60,
                        height: '250px',
                        width: '90%'
                    }
                };

                var materialChart = new google.charts.Bar(document.getElementById(nomeDiv));
                materialChart.draw(data, options);

                $("#" + nomeDiv + "_title").show();
                
            }
            
        }
    });

}






function IncidentesPessoasPorTipoAcidente(nomeDiv) {

    $("#" + nomeDiv).css("width", $("#" + nomeDiv).parent().width() + "px");

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(function () { LoadIncidentesPessoasPorTipoAcidente(nomeDiv); });
}


function LoadIncidentesPessoasPorTipoAcidente(nomeDiv) {

    $.ajax({
        method: "POST",
        url: "/Home/LoadIncidentesPessoasPorTipoAcidente",
        data: {},
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            TratarResultadoJSON(content.resultado);

            if (content.resultado.Conteudo.length > 0) {

                var data = google.visualization.arrayToDataTable(eval("[" + content.resultado.Conteudo + "]"));
                var options = {
                    title: ''
                };
                var chart = new google.visualization.PieChart(document.getElementById(nomeDiv));
                chart.draw(data, options);

                $("#" + nomeDiv + "_title").show();
            }
        }
    });
}



function IncidentesVeiculosPorTipoAcidente(nomeDiv) {

    $("#" + nomeDiv).css("width", $("#" + nomeDiv).parent().width() + "px");

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(function () { LoadIncidentesVeiculosPorTipoAcidente(nomeDiv); });
}

function LoadIncidentesVeiculosPorTipoAcidente(nomeDiv) {

    $.ajax({
        method: "POST",
        url: "/Home/LoadIncidentesVeiculosPorTipoAcidente",
        data: {},
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            TratarResultadoJSON(content.resultado);

            if (content.resultado.Conteudo.length > 0) {

                var data = google.visualization.arrayToDataTable(eval("[" + content.resultado.Conteudo + "]"));
                var options = {
                    title: ''
                };
                var chart = new google.visualization.PieChart(document.getElementById(nomeDiv));
                chart.draw(data, options);

                $("#" + nomeDiv + "_title").show();
            }

        }
    });

}