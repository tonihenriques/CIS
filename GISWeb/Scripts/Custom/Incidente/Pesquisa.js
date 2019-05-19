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



    $('#timepicker1').timepicker({
        minuteStep: 1,
        showSeconds: false,
        showMeridian: false,
        disableFocus: true,
        icons: {
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down'
        }
    }).on('focus', function () {
        $('#timepicker1').timepicker('showWidget');
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
        });



    Chosen();

});



function OnBeginPesquisaDadosBase() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisa").show();
}

function OnSuccessPesquisaDadosBase(data) {
    $(".LoadingLayout").hide();
    $("#formPesquisa").hide();
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
        $("#formPesquisa").show();
    });
}

function OnFailurePesquisaDadosBase() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisa").show();
    $("#ResultadoPesquisa").html("");
}



function OnBeginPesquisaCAT() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisa").show();
}

function OnSuccessPesquisaCAT(data) {
    $(".LoadingLayout").hide();
    $("#formPesquisa").hide();
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
}

function OnFailurePesquisaCAT() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisa").show();
    $("#ResultadoPesquisa").html("");
}



function OnBeginPesquisaCodificacao() {
    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');
    $("#ResultadoPesquisa").html("");
    $("#formPesquisa").show();
}

function OnSuccessPesquisaCodificacao(data) {
    $(".LoadingLayout").hide();
    $("#formPesquisa").hide();
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
}

function OnFailurePesquisaCodificacao() {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);
    $("#formPesquisa").show();
    $("#ResultadoPesquisa").html("");
}