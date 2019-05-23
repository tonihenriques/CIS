
function OnClickAprovarIncidente(origemElemento) {
    $('#modalAprovarX').show();
    $('#modalAprovarFechar').removeClass('disabled');
    $('#modalAprovarFechar').removeAttr('disabled', 'disabled');
    $('#modalAprovarProsseguir').removeClass('disabled');
    $('#modalAprovarProsseguir').removeAttr('disabled', 'disabled');
    $('#modalAprovarProsseguir').hide();
    $('#modalAprovarCorpo').html('');
    $('#modalAprovarCorpoLoading').show();

    var ficha = origemElemento.closest('[data-uniquekey]').attr('data-uniquekey');

    $.ajax({
        method: 'GET',
        url: '/Inbox/AprovarIncidente',
        data: { uk: ficha },
        error: function (erro) {
            $('#modalAprovar').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('#modalAprovarCorpoLoading').hide();
            $('#modalAprovarLoading').hide();
            $('#modalAprovarCorpo').html(content);

            AplicaTooltip();

            var labelAprovador = $('#labelSemAprovador');

            if (labelAprovador.length === 0) {
                $('#modalAprovarProsseguir').show();

                var aprovar = function (e) {
                    $('#modalAprovarX').hide();
                    $('#modalAprovarFechar').addClass('disabled');
                    $('#modalAprovarFechar').attr('disabled', 'disabled');
                    $('#modalAprovarProsseguir').addClass('disabled');
                    $('#modalAprovarProsseguir').attr('disabled', 'disabled');

                    $('.aRemoverValidador').addClass('grey');
                    $('.aRemoverValidador').attr('disabled', true);

                    $('#formAprovacao').submit();
                };

                $('#modalAprovarProsseguir').on('click', aprovar);

                $('#modalAprovarFechar').on('click', function () {
                    $('#modalAprovarCorpo').html('');
                });

                $('#modalAprovar').on('hide', function () {
                    $('#modalAprovarProsseguir').off('click', aprovar);
                });
            }
        }
    });
}

function OnSuccessAprovar(content) {
    $("#modalAprovarX").show();
    $("#modalAprovarFechar").removeClass("disabled");
    $("#modalAprovarFechar").removeAttr("disabled", "disabled");
    $("#modalAprovarProsseguir").removeClass("disabled");
    $("#modalAprovarProsseguir").removeAttr("disabled", "disabled");

    $("#modalAprovar").modal("hide");
    $("#modalAprovarCorpo").html("");

    ExibirMensagemGritterTodos(content, "Ocorreu algum problema não identificado ao aprovar o documento.");

    //AtualizarTelasDetalhes();
    
    $("#modalDetalhesIncidente").modal("hide");
    $("#ObjRecemCriado").val("");
    
    if ($(".msgRetornoExterno .alert").length > 0) {
        $(".msgRetornoExterno .alert").hide();
    }

    AtualizarIncidentes();

}


function OnClickAssumirIncidente(origemElemento) {
    $('#modalAssumirX').show();
    $('#modalAssumirFechar').removeClass('disabled');
    $('#modalAssumirFechar').removeAttr('disabled', 'disabled');
    $('#modalAssumirProsseguir').removeClass('disabled');
    $('#modalAssumirProsseguir').removeAttr('disabled', 'disabled');
    $('#modalAssumirProsseguir').hide();
    $('#modalAssumirCorpo').html('');
    $('#modalAssumirCorpoLoading').show();

    var ficha = origemElemento.closest('[data-uniquekey]').attr('data-uniquekey');

    $.ajax({
        method: 'GET',
        url: '/Inbox/Assumir',
        data: { uk: ficha },
        error: function (erro) {
            $('#modalAssumir').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('#modalAssumirCorpoLoading').hide();
            $('#modalAssumirLoading').hide();
            $('#modalAssumirCorpo').html(content);

            AplicaTooltip();

            var labelAprovador = $('#labelSemAprovador');

            if (labelAprovador.length === 0) {
                $('#modalAssumirProsseguir').show();

                var aprovar = function (e) {
                    $('#modalAssumirX').hide();
                    $('#modalAssumirFechar').addClass('disabled');
                    $('#modalAssumirFechar').attr('disabled', 'disabled');
                    $('#modalAssumirProsseguir').addClass('disabled');
                    $('#modalAssumirProsseguir').attr('disabled', 'disabled');

                    $('.aRemoverValidador').addClass('grey');
                    $('.aRemoverValidador').attr('disabled', true);

                    $('#formAprovacao').submit();
                };

                $('#modalAssumirProsseguir').on('click', aprovar);

                $('#modalAssumirFechar').on('click', function () {
                    $('#modalAssumirCorpo').html('');
                });

                $('#modalAssumir').on('hide', function () {
                    $('#modalAprovarProsseguir').off('click', aprovar);
                });
            }
        }
    });
}

function OnSuccessAssumir(content) {
    $("#modalAssumirX").show();
    $("#modalAssumirFechar").removeClass("disabled");
    $("#modalAssumirFechar").removeAttr("disabled", "disabled");
    $("#modalAssumirProsseguir").removeClass("disabled");
    $("#modalAssumirProsseguir").removeAttr("disabled", "disabled");

    $("#modalAssumir").modal("hide");
    $("#modalAssumirCorpo").html("");

    ExibirMensagemGritterTodos(content, "Ocorreu algum problema não identificado ao aprovar o documento.");

    AtualizarTelasDetalhes();

    AtualizarIncidentes();

}



function OnClickRejeitarIncidente(origemElemento) {
    $('#modalRejeitarX').show();
    $('#modalRejeitarFechar').removeClass('disabled');
    $('#modalRejeitarFechar').removeAttr('disabled', 'disabled');
    $('#modalRejeitarProsseguir').removeClass('disabled');
    $('#modalRejeitarProsseguir').removeAttr('disabled', 'disabled');
    $('#modalRejeitarProsseguir').hide();
    $('#modalRejeitarCorpo').html('');
    $('#modalRejeitarCorpoLoading').show();

    var ficha = origemElemento.closest('[data-uniquekey]').attr('data-uniquekey');

    $.ajax({
        method: 'GET',
        url: '/Inbox/RejeitarIncidente',
        data: { uk: ficha },
        error: function (erro) {
            $('#modalRejeitar').modal('hide');
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
        },
        success: function (content) {
            $('#modalRejeitarCorpoLoading').hide();
            $('#modalRejeitarLoading').hide();
            $('#modalRejeitarCorpo').html(content);

            AplicaTooltip();

            var labelAprovador = $('#labelSemAprovador');
            if (labelAprovador.length === 0) {
                $('#modalRejeitarProsseguir').show();

                var aprovar = function (e) {
                    $('#modalRejeitarX').hide();
                    $('#modalRejeitarFechar').addClass('disabled');
                    $('#modalRejeitarFechar').attr('disabled', 'disabled');
                    $('#modalRejeitarProsseguir').addClass('disabled');
                    $('#modalRejeitarProsseguir').attr('disabled', 'disabled');

                    $('.aRemoverValidador').addClass('grey');
                    $('.aRemoverValidador').attr('disabled', true);

                    $('#formAprovacao').submit();
                };

                $('#modalRejeitarProsseguir').on('click', aprovar);

                $('#modalRejeitarFechar').on('click', function () {
                    $('#modalRejeitarCorpo').html('');
                });

                $('#modalRejeitar').on('hide', function () {
                    $('#modalRejeitarProsseguir').off('click', aprovar);
                });
            }
        }
    });
}

function OnSuccessRejeitar(content) {
    $("#modalRejeitarX").show();
    $("#modalRejeitarFechar").removeClass("disabled");
    $("#modalRejeitarFechar").removeAttr("disabled", "disabled");
    $("#modalRejeitarProsseguir").removeClass("disabled");
    $("#modalRejeitarProsseguir").removeAttr("disabled", "disabled");

    $("#modalRejeitar").modal("hide");
    $("#modalRejeitarCorpo").html("");

    ExibirMensagemGritterTodos(content, "Ocorreu algum problema não identificado ao rejeitar.");

    //AtualizarTelasDetalhes();
    $("#modalDetalhesIncidente").modal("hide");

    if ($(".msgRetornoExterno .alert").length > 0) {
        $(".msgRetornoExterno .alert").hide();
    }

    $("#ObjRecemCriado").val("");

    AtualizarIncidentes();
}