jQuery(function ($) {

    DatePTBR();

    var date = new Date();
    date.setDate(date.getDate());

    Chosen();

});

function OnBeginPesquisaCodificacao() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisaCodificacao").show();
}

function OnSuccessPesquisaCodificacao(data) {
    if (data.resultado != null &&
        data.resultado != undefined &&
        data.resultado.Erro != null &&
        data.resultado.Erro != undefined &&
        data.resultado.Erro != "") {

        $('.page-content-area').ace_ajax('stopLoading', true);
        $(".LoadingLayout").hide();
        ExibirMensagemDeAlerta(data.resultado.Erro);
    }
    else {

        $(".LoadingLayout").hide();
        $("#formPesquisaCodificacao").hide();
        $('.page-content-area').ace_ajax('stopLoading', true);
        $("#ResultadoPesquisa").html(data);

        AplicaTooltip();

        if ($("#divTableResultadoPesquisa").length > 0) {
            AplicajQdataTable("tableResultadoPesquisa", [null, null, null, null, null, null, null, null, { "bSortable": false }], false, 20);

            $('.btnDropdownMenu').off("click").on('click', function () {
                $(this).closest('tr').css({ 'background-color': '#dff0d8' });

                OnClickBtnDropdownMenu($(this));
            });
        }

        $(".btnVoltarPesquisaBase").off("click").on("click", function () {
            $("#ResultadoPesquisa").html("");
            $("#formPesquisaCodificacao").show();
        });
    }
}

function OnFailurePesquisaCodificacao() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisaCodificacao").show();
    $("#ResultadoPesquisa").html("");
}