
function MeusIncidentesChartBar(nomeDiv) {

    $("#" + nomeDiv).css("width", $("#" + nomeDiv).parent().width() + "px");

    google.charts.load('current', { packages: ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(function () { LoadMeusIncidentesChartBar(nomeDiv); });
}

function LoadMeusIncidentesChartBar(nomeDiv) {

    $.ajax({
        method: "POST",
        url: "/Home/LoadMeusIncidentesBar",
        data: {  },
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            TratarResultadoJSON(content.resultado);

            if (content.resultado.Conteudo.length > 0) {

                //var code = "";
                //code += "var data = google.visualization.arrayToDataTable([";
                //code += content.resultado.Conteudo;
                //code += "]);";

                //alert(content.resultado.Conteudo);

                //code += "var options = {";
                //code += "    title: 'Total Incidentes por Mês',";
                //code += "    chartArea: {";
                //code += "        backgroundColor: 'transparent',";
                //code += "        left: 60,";
                //code += "        height: '250px',";
                //code += "        width: '100%'";
                //code += "    }";
                //code += "};";

                //code += "var materialChart = new google.charts.Bar(document.getElementById(" + nomeDiv + "));";
                //code += "materialChart.draw(data, options);";

                //eval(code);

                //var data = google.visualization.arrayToDataTable([
                //    ['Mês', 'Pessoa', { role: 'annotation' }, 'Veículo', { role: 'annotation' }],
                //    ['Janeiro', 9, '9', 6, '6'],
                //    ['Fevereiro', 10, '10', 2, '2'],
                //    ['Março', 11, '11', 4, '4'],
                //    ['Abril', 12, '12', 4, '4'],
                //    ['Maio', 15, '15', 5, '5'],
                //    ['Junho', 12, '12', 4, '4']
                //]);


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

function MeusIncidentesPorTipoAcidente(nomeDiv) {

    $("#" + nomeDiv).css("width", $("#" + nomeDiv).parent().width() + "px");

    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(function () { LoadMeusIncidentesPorTipoAcidente(nomeDiv); });
}


function LoadMeusIncidentesPorTipoAcidente(nomeDiv) {

    $.ajax({
        method: "POST",
        url: "/Home/LoadMeusIncidentesPorTipoAcidente",
        data: {},
        error: function (erro) {
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            TratarResultadoJSON(content.resultado);

            if (content.resultado.Conteudo.length > 0) {

                //var code = "";
                //code += "var data = google.visualization.arrayToDataTable([";
                //code += content.resultado.Conteudo;
                //code += "]);";

                //code += "var options = {";
                //code += "    title: 'Total Incidentes por Tipo Acidente',";
                //code += "};";

                //code += "var chart = new google.visualization.PieChart(document.getElementById(" + nomeDiv + "));";
                //code += "chart.draw(data, options);";

                //eval(code);


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