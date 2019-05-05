jQuery(function ($) {

    var fInbox = $("#FuncaoInbox").val();

    if (fInbox == "Incidentes") {
        AtualizarIncidentes();
    }
    else if (fInbox == "IncidentesVeiculos") {
        AtualizarIncidentesVeiculos();
    }
    else if (fInbox == "QuaseIncidentes") {
        AtualizarQuaseIncidentes();
    }
    else if (fInbox == "QuaseIncidentesVeiculos") {
        AtualizarQuaseIncidentesVeiculos();
    }

});

function AtualizarIncidentes() {

    $("#result_incidente").html("");
    $(".LoadingLayout").show();

    if (EstaNoModalVisualizarDetalhesIncidente()) {
        $("#modalDetalhesIncidenteCorpoLoading").show();
        $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Atualizando tela');
        BloquearDiv("modalDetalhesIncidente");
    }
    else {
        $('.page-content-area').ace_ajax('startLoading');
    }
    

    $.ajax({
        method: "POST",
        url: "/Inbox/CarregarInboxPessoal",
        data: { tab: "Incidentes" },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            $("#modalDetalhesIncidenteCorpoLoading").hide();
            $('#modalDetalhesIncidenteCorpoLoadingTexto').html('');
            DesbloquearDiv("modalDetalhesIncidente");

            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            $("#modalDetalhesIncidenteCorpoLoading").hide();
            $('#modalDetalhesIncidenteCorpoLoadingTexto').html('');
            DesbloquearDiv("modalDetalhesIncidente");

            if (content.resultado != null && content.resultado != undefined) {
                TratarResultadoJSON(content.resultado);
            }            

            $("#result_incidente").html(content);

            if ($("#tableResultadoIncidentes").length > 0) {
                AplicaTooltip();

                AplicajQdataTable("tableResultadoIncidentes", [null, null, null, null, null, null, null, null, { "bSortable": false }], false, 20);

                if ($("#ObjRecemCriado").val() != "") {
                    $('#modalDetalhesIncidente').modal('show');
                    var obj = $("#" + $("#ObjRecemCriado").val());
                    VisualizarDetalhesIncidente(obj);
                }

                $('.btnDropdownMenu').off("click").on('click', function () {
                    $(this).closest('tr').css({ 'background-color': '#dff0d8' });

                    OnClickBtnDropdownMenu($(this));
                });

            }

        }
    });

}

function AtualizarIncidentesVeiculos() {

    $("#result_incidente_veiculo").html("");
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');

    $.ajax({
        method: "POST",
        url: "/Inbox/CarregarInboxPessoal",
        data: { tab: "IncidentesVeiculos" },
        error: function (erro) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            $('.page-content-area').ace_ajax('stopLoading', true);

            if (content.resultado != null && content.resultado != undefined) {
                TratarResultadoJSON(content.resultado);
            }

            $("#result_incidente_veiculo").html(content);

            if ($("#tableResultadoIncidentesVeiculos").length > 0) {
                AplicaTooltip();

                AplicajQdataTable("tableResultadoIncidentesVeiculos", [null, null, null, null, null, null, null, { "bSortable": false }], false, 20);
            }

        }
    });

}
