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

}

function OnSuccessPesquisaDadosBase(data) {
    alert(data);
}

function OnFailurePesquisaDadosBase() {

}



function OnBeginPesquisaCAT() {

}

function OnSuccessPesquisaCAT(data) {
    alert(data);
}

function OnFailurePesquisaCAT() {

}



function OnBeginPesquisaCodificacao() {

}

function OnSuccessPesquisaCodificacao(data) {
    alert(data);
}

function OnFailurePesquisaCodificacao() {

}