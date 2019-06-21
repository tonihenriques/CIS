jQuery(function ($) {

    DatePTBR();

    var date = new Date();
    date.setDate(date.getDate());

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR',
        maxDate: date
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    Chosen();

});

//###################################################################################################################

function OnBeginPesquisaEnvolvidosProprio() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisaEnvolvidosProprio").show();
}

function OnSuccessPesquisaEnvolvidosProprio(data) {
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
        $("#formPesquisaEnvolvidosProprio").hide();
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
            $("#formPesquisaEnvolvidosProprio").show();
        });
    }
}

function OnFailurePesquisaEnvolvidosProprio() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisaEnvolvidosProprio").show();
    $("#ResultadoPesquisa").html("");
}


//###################################################################################################################


function OnBeginPesquisaEnvolvidosTerceiro() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisaEnvolvidosTerceiro").show();
}

function OnSuccessPesquisaEnvolvidosTerceiro(data) {
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
        $("#formPesquisaEnvolvidosTerceiro").hide();
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
            $("#formPesquisaEnvolvidosTerceiro").show();
        });
    }
}

function OnFailurePesquisaEnvolvidosTerceiro() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisaEnvolvidosTerceiro").show();
    $("#ResultadoPesquisa").html("");
}