jQuery(function ($) {
    DatePTBR();  
});

function VisualizarDetalhesIncidente(elementoclicado) {

    var codigoIncidente = $(elementoclicado).closest("[data-codigo]").attr("data-codigo");
    var ukIncidente = $(elementoclicado).closest("[data-uniquekey]").attr("data-uniquekey");

    $('#modalDetalhesIncidenteTituloName').html('[<a href="#" class="lnkRevisaoEspecifica blue" data-target="#modalObterLinkEsp" data-toggle="modal">' + codigoIncidente + '</a>]');

    $('#modalDetalhesIncidenteTitulo').html('');
    $('#modalDetalhesIncidenteTitulo').hide();

    $('#modalDetalhesIncidenteCorpo').attr('data-codigo', codigoIncidente);
    $('#modalDetalhesIncidenteCorpo').attr('data-uniquekey', ukIncidente);

    $('#modalDetalhesIncidenteX').show();
    $('#modalDetalhesIncidenteCorpo').html('');
    $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Carregando dados do incidente');
    $('#modalDetalhesIncidenteCorpoLoading').show();

    $.post('/Incidente/Detalhes', { uniquekey: ukIncidente }, function (content) {
        $('#modalDetalhesIncidenteCorpoLoading').hide();

        if (content.resultado != null && content.resultado != undefined) {

            if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                var divErro = "" +
                    "<div class=\"alert alert-danger\">" +
                    "<strong>" +
                    "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                    "Oops! " +
                    "</strong>" +

                    "<span>" + content.resultado.Erro + "</span>" +
                    "<br />" +
                    "</div>";

                $('#modalDetalhesIncidenteCorpo').html(divErro);

            }
        } else {

            $('#modalDetalhesIncidenteCorpo').html(content);

            AplicaTooltip();

            AdicionarFuncoesOnClikParaOperacoes();

            if ($("#tableEnvolvidos").length > 0) {
                AplicajQdataTable("tableEnvolvidos", [{ "bSortable": false }, null, null, null, { "bSortable": false }], false, 20);
            }

        }
    });

}

function OnClickBtnDropdownMenu(elementoClicado) {

    var ficha = elementoClicado.closest('[data-uniquekey]').attr('data-uniquekey');

    var loading = '' +
        '<li class="center">' +
        '<img src="' + baseApplicationURL + '/Images/slack_loading.gif" width="32px" />' +
        '</li>';

    $("#menuOperacoes-" + ficha).html(loading);

    $.ajax({
        method: 'POST',
        url: '/Incidente/MontarMenuDeOperacoes',
        data: { uk: ficha },
        error: function (erro) {
            $('#menuOperacoes-' + ficha.replace("/", "")).html("<li class=\"action-buttons\">" +
                "<a>" +
                "<span class=\"lnkSemOperacao orange2 CustomTooltip\" title=\"Nenhuma operação disponível\">" +
                "    <i class=\"ace-icon fa fa-exclamation-triangle bigger-150\"></i>" +
                "</span>" +
                "</a>" +
                "</li>");
            ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('#menuOperacoes-' + ficha.replace("/", "")).html(content);
            AplicaTooltip();
            AdicionarFuncoesOnClikParaOperacoes();
        }
    });
}

function AdicionarFuncoesOnClikParaOperacoes() {
    $('.lnkAprovar').on('click', function () {
        OnClickAprovarIncidente($(this));
    });

    $('.lnkAssumir').on('click', function () {
        OnClickAssumirIncidente($(this));
    });

    $('.lnkReprovar').on('click', function () {
        OnClickRejeitarIncidente($(this));
    });

    $('.lnkUploadArquivo').off("click").on('click', function (e) {
        e.preventDefault();

        var btnUploadArquivo = $(this);
        
        $('#modalArquivoX').show();

        $('#modalArquivoFechar').removeClass('disabled');
        $('#modalArquivoFechar').removeAttr('disabled');
        $('#modalArquivoFechar').on('click', function (e) {
            e.preventDefault();
            $('#modalArquivo').modal('hide');
        });

        $('#modalArquivoProsseguir').hide();

        $('#modalArquivoCorpo').html('');
        $('#modalArquivoCorpoLoading').show();

        $.ajax({
            method: "GET",
            url: "/Arquivo/Upload",
            data: { ukObjeto: btnUploadArquivo.closest("[data-uniquekey]").data("uniquekey") },
            error: function (erro) {
                $('#modalArquivo').modal('hide');
                ExibirMensagemGritter('Oops!', erro.responseText, 'gritter-error');
            },
            success: function (content) {
                $('#modalArquivoCorpoLoading').hide();
                $('#modalArquivoCorpo').html(content);

                InitDropZoneSingle(btnUploadArquivo);

                Chosen();

                $.validator.unobtrusive.parse('#formUpload');
            }
        });
    });

    $('.lnkExcluirArquivo').on('click', function (e) {
        e.preventDefault();

        OnClickExcluirArquivo($(this));
    });

    $('.lnkDownloadTodosArquivos').on('click', function (e) {
        e.preventDefault();
        OnClickDownloadTodosArquivos($(this));
    });

    $(".lnkNovoEmpregadoProprio").off("click").on('click', function(e) {
        e.preventDefault();
        OnClickNovoEmpProprio($(this));
    });

    $(".lnkNovoEmpregadoTerceiro").off("click").on('click', function (e) {
        e.preventDefault();
        OnClickNovoEmpTerceiro($(this));
    });

    $(".lkEditProprio").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickEditarEmpProprio($(this));
    });

    $(".lkEditTerceiro").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickEditarEmpTerceiro($(this));
    });

    $(".lnkExcluirIncidente").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickExcluirIncidente($(this));
    });

    $(".lnkAlterarDados").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickNovaCodificacao($(this));
    });

    $(".lkNovaCodificacao").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickNovaCodificacao($(this));
    });

    $(".lkEditarCodificacao").off("click").on("click", function (e) {
        e.preventDefault();
        OnClickEditarCodificacao($(this));
    });

}

function AtualizarTelasDetalhes() {
    if (EstaNaCaixaDeEntrada()) {
        if (EstaNoModalVisualizarDetalhesIncidente())
            VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));

        AtualizarIncidentes();
    } else if (EstaNaPesquisaIncidente()) {
        if (EstaNoModalVisualizarDetalhesIncidente())
            VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));

        AtualizarRowPesquisa();
    }
}



function EstaNaCaixaDeEntrada() {
    return $('#widgetBodyInbox').length !== 0;
}

function EstaNaPesquisaIncidente() {
    return $('#divPesquisaResultado').length !== 0 || $('#divPesquisaDireta').length !== 0;
}

function EstaNoModalVisualizarDetalhesIncidente() {
    return ($('#modalDetalhesIncidente').data('bs.modal') || { isShown: false }).isShown;
}

function InitDropZoneSingle(elementoClicado) {
    try {
        Dropzone.autoDiscover = false;

        var dictDefaultMessage = "";
        dictDefaultMessage += '<span class="bigger-150 bolder">';
        dictDefaultMessage += '  <i class="ace-icon fa fa-caret-right red"></i> Arraste arquivos</span> para upload \
				                <span class="smaller-80 grey">(ou clique)</span> <br /> \
				                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-3x"></i>';

        var previewTemplate = "";
        previewTemplate += "<div class=\"dz-preview dz-file-preview\">\n ";
        previewTemplate += "    <div class=\"dz-details\">\n ";
        previewTemplate += "        <div class=\"dz-filename\">";
        previewTemplate += "            <span data-dz-name></span>";
        previewTemplate += "        </div>\n    ";
        previewTemplate += "        <div class=\"dz-size\" data-dz-size></div>\n  ";
        previewTemplate += "        <img data-dz-thumbnail />\n  ";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"progress progress-small progress-striped active\">";
        previewTemplate += "        <div class=\"progress-bar progress-bar-success\" data-dz-uploadprogress></div>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-success-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-message\">";
        previewTemplate += "        <span data-dz-errormessage></span>";
        previewTemplate += "    </div>\n";
        previewTemplate += "</div>";

        //#######################################################################################################
        //Recupera do form montado os respectivos valores retornados do servidor e armazenados na web como 'data'
        var extensoes = $('#formUpload').data('extensoes');
        if (extensoes == '')
            extensoes = null;

        var uploadMultiplo = $('#formUpload').data('uploadmultiplo');
        /*var maxArquivos = 1;
        if (uploadMultiplo && uploadMultiplo.toUpperCase() == 'TRUE')
            maxArquivos = 200;*/

        var maxArquivos = 200;

        var tamanhoMaximo = $('#formUpload').data('tamanhomaximo');
        //#######################################################################################################

        var myDropzone = new Dropzone("#formUpload", {
            paramName: "file",
            uploadMultiple: false, //se habilitar upload múltiplo, pode bugar o SPF
            parallelUploads: 1, //se for mais que 1, pode bugar o SPF
            maxFilesize: tamanhoMaximo, // MB
            dictFileTooBig: 'Tamanho máximo permitido ultrapassado.',
            maxFiles: maxArquivos,
            dictMaxFilesExceeded: 'Limite máximo de número de arquivos permitidos ultrapassado.',
            acceptedFiles: extensoes,
            dictInvalidFileType: 'Extensão de arquivo inválida para este tipo de anexo.',
            addRemoveLinks: true,
            dictCancelUpload: 'Cancelar',
            dictCancelUploadConfirmation: 'Tem certeza que deseja cancelar?',
            dictRemoveFile: 'Remover',
            dictDefaultMessage: dictDefaultMessage,
            dictResponseError: 'Erro ao fazer o upload do arquivo.',
            dictFallbackMessage: 'Este browser não suporta a funcionalidade de arrastar e soltar arquivos para fazer upload.',
            previewTemplate: previewTemplate,
        });

        myDropzone.on('sending', function (file) {
            if (!$('#formUpload').valid()) {
                myDropzone.removeFile(file);
            } else {
                $('#modalArquivoX').hide();
                $('#modalArquivoFechar').addClass('disabled');
                $('#modalArquivoFechar').attr('disabled', 'disabled');
            }
        });

        myDropzone.on('canceled', function () {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('success', function (file, content) {
            if (content.sucesso || content.alerta) {
                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalArquivo').modal('hide');

                    $('#detalhesFicha').css({ opacity: "0.5" });

                    $('#detalhesFichaLoadingTexto').html('...Carregando dados do documento');
                    $('#modalDetalhesFichaCorpoLoadingTexto').html('...Carregando dados do documento');

                    if ($('#formVisualizarDetalhes').length !== 0)
                        $('#formVisualizarDetalhes').submit();
                    else
                        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
                }
            }
            else {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
                $(".dz-success-mark").hide();
                $(".dz-error-mark").show();

                if (content.erro) {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    ExibirMensagemGritter('Oops!', 'Ocorreu algum problema não identificado ao fazer o upload do arquivo para o servidor.', 'gritter-error');
                }
                    
            }
        });

        myDropzone.on('error', function () {
            $('#modalArquivoX').show();
            $('#modalArquivoFechar').removeClass('disabled');
            $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
        });

        myDropzone.on('removedfile', function (file) {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('maxfilesexceeded', function () {
            ExibirMensagemGritter('Alerta', 'Só é permitida a inclusão de 1 arquivo para cada tipo de anexo.', 'gritter-warning');
        });

        $(document).one('ajaxloadstart.page', function (e) {
            try {
                myDropzone.destroy();
            } catch (e) { }
        });

    } catch (e) {
        ExibirMensagemGritter('Alerta', 'Este browser não é compatível com o componente Dropzone.js. Sugerimos a utilização do Google Chrome ou Internet Explorer 10 (ou versão superior).', 'gritter-warning');
    }
}

function OnClickExcluirArquivo(origemElemento) {
    var excluir = function () {
        $('#detalhesIncidenteLoadingTexto').html('...Excluindo arquivo');
        $('#detalhesIncidenteLoading').show();

        $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Excluindo arquivo');
        $('#modalDetalhesIncidenteCorpoLoading').show();

        BloquearDiv("detalhesIncidente");

        $.ajax({
            method: "POST",
            url: "/Arquivo/Excluir",
            data: { ukArquivo: origemElemento.closest("[data-uniquekey]").data("uniquekey") },
            success: function (content) {
                $('#detalhesIncidenteLoading').hide();
                $('#modalDetalhesIncidenteCorpoLoading').hide();

                DesbloquearDiv("detalhesIncidente");

                if (content.erro) {
                    ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
                }
                else {
                    if ($('#formVisualizarDetalhes').length !== 0) {
                        $('#conteudoDetalhesIncidente').html('');
                        $('#detalhesIncidenteLoadingTexto').html('...Carregando dados do documento');
                        $('#formVisualizarDetalhes').submit();
                    }
                    else
                        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
                }
            }
        });
    }

    var msg = "Você está excluindo este arquivo.";
    var classeBotao = "btn-danger";
    var titulo = "Exclusão de Arquivo";

    ExibirMensagemDeConfirmacaoSimples(msg, titulo, excluir, classeBotao);
}

function OnClickDownloadTodosArquivos(origemElemento) {
    $('#detalhesIncidenteLoadingTexto').html('...Gerando pacote zip dos arquivos');
    $('#detalhesIncidenteLoading').show();

    $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Gerando pacote zip dos arquivos');
    $('#modalDetalhesIncidenteCorpoLoading').show();

    $('#detalhesFicha').css({ opacity: "0.5" });

    $.ajax({
        method: "POST",
        url: "/Arquivo/DownloadTodosArquivos",
        data: { id: origemElemento.closest("[data-uniquekey]").data("uniquekey") },
        success: function (content) {
            $('#detalhesIncidenteLoading').hide();
            $('#modalDetalhesIncidenteCorpoLoading').hide();

            $('#detalhesIncidente').removeAttr('style');

            if (content.url) {
                window.open(content.url, '_blank');
            } else if (content.erro) {
                ExibirMensagemGritter('Oops!', content.erro, 'gritter-error');
            } else {
                ExibirMensagemGritter('Oops!', "Não foi possível gerar o pacote zip de todos os arquivos para download.", 'gritter-error');
            }
        }
    });
}

function OnClickNovoEmpProprio(origemElemento) {
    
    $('#modalNewEmpPropCorpo').html('');
    $('#modalNewEmpPropCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalNewEmpPropCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/EmpregadoProprio/Novo",
        data: { id: origemElemento.closest("[data-uniquekey]").data("uniquekey") },
        success: function (content) {
            $('#modalNewEmpPropCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalNewEmpPropCorpo').html(divErro);

                }
            } else {

                $('#modalNewEmpPropCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalNewEmpPropProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formCadastroEmpregado").submit();
                });

            }
        }
    });
}

function OnClickNovoEmpTerceiro(origemElemento) {
    
    $('#modalNewEmpTercCorpo').html('');
    $('#modalNewEmpTercCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalNewEmpTercCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/EmpregadoContratado/Novo",
        data: { id: origemElemento.closest("[data-uniquekey]").data("uniquekey") },
        success: function (content) {
            $('#modalNewEmpTercCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {
                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalNewEmpTercCorpo').html(divErro);
                }
            } else {

                $('#modalNewEmpTercCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalNewEmpTercProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formCadastroContratado").submit();
                });

            }
        }
    });
}



function OnBeginCadastrarEmpProprio() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });

    $('#modalNewEmpPropLoading').show();
    BloquearDiv("modalNewEmpProp");
}

function OnSuccessCadastrarEmpProprio(data) {
    $('#formCadastroEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    $('#modalNewEmpPropLoading').hide();
    DesbloquearDiv("modalNewEmpProp");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalNewEmpProp').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnBeginCadastrarEmpContratado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroContratado").css({ opacity: "0.5" });
    
    $('#modalNewEmpTercLoading').show();
    BloquearDiv("modalNewEmpTerc");
}

function OnSuccessCadastrarEmpContratado(data) {
    $('#formCadastroContratado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    $('#modalNewEmpTercLoading').hide();
    DesbloquearDiv("modalNewEmpTerc");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalNewEmpTerc').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }

}


function ExcluirEnvolvidoProprio(ukRel, nomeEnvolvido) {

    var excluir = function () {
        $('#detalhesIncidenteLoadingTexto').html('...Excluindo envolvido');
        $('#detalhesIncidenteLoading').show();

        $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Excluindo envolvido');
        $('#modalDetalhesIncidenteCorpoLoading').show();

        BloquearDiv("detalhesIncidente");

        $.ajax({
            method: "POST",
            url: "/EmpregadoProprio/Excluir",
            data: { UKRel: ukRel, Nome: nomeEnvolvido },
            success: function (content) {
                $('#detalhesIncidenteLoadingTexto').html('');
                $('#detalhesIncidenteLoading').hide();

                $('#modalDetalhesIncidenteCorpoLoadingTexto').html('');
                $('#modalDetalhesIncidenteCorpoLoading').hide();

                DesbloquearDiv("detalhesIncidente");

                if (content.resultado.Erro != undefined && content.resultado.Erro != null && content.resultado.Erro != "") {
                    ExibirMensagemGritter('Oops!', content.resultado.Erro, 'gritter-error');
                }
                else {
                    ExibirMensagemDeSucesso(content.resultado.Sucesso);
                    VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
                }
            }
        });
    };

    var msg = "Você está excluindo o envolvido '" + nomeEnvolvido + "' deste incidente.";
    var classeBotao = "btn-danger";
    var titulo = "Exclusão de Envolvido Próprio";

    ExibirMensagemDeConfirmacaoSimples(msg, titulo, excluir, classeBotao);

}

function ExcluirEnvolvidoTerceiro(ukRel, nomeEnvolvido) {

    var excluir = function () {
        $('#detalhesIncidenteLoadingTexto').html('...Excluindo envolvido');
        $('#detalhesIncidenteLoading').show();

        $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Excluindo envolvido');
        $('#modalDetalhesIncidenteCorpoLoading').show();

        BloquearDiv("detalhesIncidente");

        $.ajax({
            method: "POST",
            url: "/EmpregadoContratado/Excluir",
            data: { UKRel: ukRel, Nome: nomeEnvolvido },
            success: function (content) {
                $('#detalhesIncidenteLoadingTexto').html('');
                $('#detalhesIncidenteLoading').hide();

                $('#modalDetalhesIncidenteCorpoLoadingTexto').html('');
                $('#modalDetalhesIncidenteCorpoLoading').hide();

                DesbloquearDiv("detalhesIncidente");

                if (content.resultado.Erro != undefined && content.resultado.Erro != null && content.resultado.Erro != "") {
                    ExibirMensagemGritter('Oops!', content.resultado.Erro, 'gritter-error');
                }
                else {
                    ExibirMensagemDeSucesso(content.resultado.Sucesso);
                    VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
                }
            }
        });
    };

    var msg = "Você está excluindo o envolvido '" + nomeEnvolvido + "' deste incidente.";
    var classeBotao = "btn-danger";
    var titulo = "Exclusão de Envolvido Terceiro";

    ExibirMensagemDeConfirmacaoSimples(msg, titulo, excluir, classeBotao);

}


function OnClickEditarEmpProprio(origemElemento) {

    $('#modalEditEmpPropCorpo').html('');
    $('#modalEditEmpPropCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalEditEmpPropCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/EmpregadoProprio/Edicao",
        data: { id: origemElemento.data("uniquekeyrel") },
        success: function (content) {
            $('#modalEditEmpPropCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalEditEmpPropCorpo').html(divErro);

                }
            } else {

                $('#modalEditEmpPropCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalEditEmpPropProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formEdicaoEmpregadoProprio").submit();
                });

            }
        }
    });
}

function OnClickEditarEmpTerceiro(origemElemento) {

    $('#modalEditEmpTercCorpo').html('');
    $('#modalEditEmpTercCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalEditEmpTercCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/EmpregadoContratado/Edicao",
        data: { id: origemElemento.data("uniquekeyrel") },
        success: function (content) {
            $('#modalEditEmpTercCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalEditEmpTercCorpo').html(divErro);

                }
            } else {

                $('#modalEditEmpTercCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalEditEmpTercProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formEdicaoEmpregadoTerc").submit();
                });

            }
        }
    });
}


function OnBeginEditarEmpProprio() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEmpregadoProprio").css({ opacity: "0.5" });

    $('#modalEditEmpPropLoading').show();
    BloquearDiv("modalEditEmpProp");
}

function OnSuccessEditarEmpProprio(data) {
    $('#formEdicaoEmpregadoProprio').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    $('#modalEditEmpPropLoading').hide();
    DesbloquearDiv("modalEditEmpProp");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalEditEmpProp').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnBeginEditarEmpTerceiro() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEmpregadoTerc").css({ opacity: "0.5" });

    $('#modalEditEmpTercLoading').show();
    BloquearDiv("modalEditEmpTerc");
}

function OnSuccessEditarEmpTerceiro(data) {
    $('#formEdicaoEmpregadoTerc').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();

    $('#modalEditEmpTercLoading').hide();
    DesbloquearDiv("modalEditEmpTerc");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalEditEmpTerc').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnClickExcluirIncidente(origemElemento) {
    var msgInformativa = "Você está excluindo este incidente.";

    var callback = function () {

        if (EstaNoModalVisualizarDetalhesIncidente()) {
            $("#modalDetalhesIncidenteCorpoLoading").show();
            $('#modalDetalhesIncidenteCorpoLoadingTexto').html('...Excluindo incidente');
            BloquearDiv("modalDetalhesIncidente");
        }
        else {
            $('.page-content-area').ace_ajax('startLoading');
        }
        
        $.ajax({
            method: "POST",
            url: "/Incidente/Terminar",
            data: { id: origemElemento.closest("[data-uniquekey]").attr("data-uniquekey") },
            success: function (content) {
                
                if (EstaNoModalVisualizarDetalhesIncidente()) {
                    $("#modalDetalhesIncidenteCorpoLoading").hide();
                    $('#modalDetalhesIncidenteCorpoLoadingTexto').html('');
                    DesbloquearDiv("modalDetalhesIncidente");
                }
                else {
                    $('.page-content-area').ace_ajax('stopLoading', true);
                }

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != undefined && content.resultado.Sucesso != "") {
                    if (EstaNoModalVisualizarDetalhesIncidente()) {
                        $('#modalDetalhesIncidente').modal('hide');
                    }

                    if (EstaNaCaixaDeEntrada()) {
                        AtualizarIncidentes();
                    }
                }
                
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples(msgInformativa, "Exclusão", callback, "btn-danger");
}



function OnClickAlterarDados(elementoclicado) {

    var codigoIncidente = $(elementoclicado).closest("[data-codigo]").attr("data-codigo");
    var ukIncidente = $(elementoclicado).closest("[data-uniquekey]").attr("data-uniquekey");

    $('#modalEditIncidenteTituloName').html('[<a href="#" class="lnkRevisaoEspecifica blue" data-target="#modalObterLinkEsp" data-toggle="modal">' + codigoIncidente + '</a>]');

    $("#modalEditIncidenteProsseguir").hide();

    $('#modalEditIncidenteTitulo').html('');
    $('#modalEditIncidenteTitulo').hide();

    $('#modalEditIncidenteCorpo').attr('data-codigo', codigoIncidente);
    $('#modalEditIncidenteCorpo').attr('data-uniquekey', ukIncidente);
    $('#modalEditIncidenteCorpo').html("");

    $('#modalEditIncidenteX').show();
    $('#mmodalEditIncidenteCorpo').html('');
    $('#modalEditIncidenteLoadingTexto').html('...Carregando dados do incidente');
    $('#modalEditIncidenteLoading').show();

    $.post('/Incidente/Edicao', { uniquekey: ukIncidente }, function (content) {
        $('#modalEditIncidenteCorpoLoading').hide();
        $("#modalEditIncidenteLoading").hide();

        if (content.resultado != null && content.resultado != undefined) {
            if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {

                

                var divErro = "" +
                    "<div class=\"alert alert-danger\">" +
                    "<strong>" +
                    "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                    "Oops! " +
                    "</strong>" +

                    "<span>" + content.resultado.Erro + "</span>" +
                    "<br />" +
                    "</div>";

                $('#modalEditIncidenteCorpo').html(divErro);

            }
        } else {

            $('#modalEditIncidenteCorpo').html(content);
            AplicaTooltip();

            $("#modalEditIncidenteProsseguir").show();

            $.validator.unobtrusive.parse(document);

            DatePTBR();

            Chosen();

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
            

            $("#UKOrgao").change(function () {

                if ($(this).val() == "") {
                    $("#UKDiretoria").val("");
                }
                else {

                    $(".LoadingLayout").show();
                    //$('.page-content-area').ace_ajax('startLoading');

                    $.ajax({
                        method: "POST",
                        url: "/Departamento/BuscarDiretoriaPorOrgao",
                        data: { ukDepartamento: $(this).val() },
                        error: function (erro) {
                            $(".LoadingLayout").hide();
                            //$('.page-content-area').ace_ajax('stopLoading', true);
                            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error');
                        },
                        success: function (content) {
                            //$('.page-content-area').ace_ajax('stopLoading', true);
                            $(".LoadingLayout").hide();

                            var resultado = content.resultado;

                            if (resultado.Erro != null && resultado.Erro != undefined && resultado.Erro != "") {
                                ExibirMensagemDeErro(resultado.Erro);
                            }
                            else if (resultado.Alerta != null && resultado.Alerta != undefined && resultado.Alerta != "") {
                                ExibirMensagemDeAlerta(resultado.Alerta);
                            }
                            else {
                                if (resultado.Conteudo.indexOf("$") != -1) {
                                    $("#UKDiretoria").val(resultado.Conteudo.substring(resultado.Conteudo.indexOf("$") + 1));
                                }
                                else {
                                    $("#UKDiretoria").val(resultado.Conteudo);
                                }
                            }

                        }
                    });

                }

            });


            $("#modalEditIncidenteProsseguir").off("click").on("click", function (e) {
                $("#formAtualizarAcidente").submit();
            });

        }
    });

}

function OnBeginAtualizarIncidente(jqXHR, settings) {
    $("#modalEditIncidenteLoading").show();
    $("#formAtualizarAcidente").css({ opacity: "0.5" });
    BloquearDiv("modalEditIncidente");

    if ($("#AcidenteFatal").prop("checked") == true) {
        $("#AcidenteFatal").val(true);
    }
    else {
        $("#AcidenteFatal").val(false);
    }

    if ($("#AcidenteGraveIP102").prop("checked") == true) {
        $("#AcidenteGraveIP102").val(true);
    }
    else {
        $("#AcidenteGraveIP102").val(false);
    }


    var form = $("#formAtualizarAcidente");
    settings.data = form.serialize();


}

function OnSuccessAtualizarIncidente(data) {
    $('#formAtualizarAcidente').removeAttr('style');
    $("#modalEditIncidenteLoading").hide();
    DesbloquearDiv("modalEditIncidente");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalEditIncidente').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnFailureAtualizarIncidente() {
    $('#formAtualizarAcidente').removeAttr('style');
    $("#modalEditIncidenteLoading").hide();
    DesbloquearDiv("modalEditIncidente");

    ExibirMensagemDeErro("Erro ao tentar atualizar o incidente. Tente novamente.");
}



function OnClickNovaCodificacao(origemElemento) {

    $('#modalNovaCodificacaoCorpo').html('');
    $('#modalNovaCodificacaoCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalNovaCodificacaoCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/Incidente/NovaCodificacao",
        data: { UKRelEnvolvido: origemElemento.data("uniquekeyrel"), Tipo: origemElemento.data("tipo") },
        success: function (content) {
            $('#modalNovaCodificacaoCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalNovaCodificacaoCorpo').html(divErro);

                }
            } else {

                $('#modalNovaCodificacaoCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalNovaCodificacaoProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formCadastroCodificacao").submit();
                });

            }
        }
    });
}

function OnBeginCadastrarCodificacao(jqXHR, settings) {
    $("#formCadastroCodificacao").css({ opacity: "0.5" });
    $("#modalNovaCodificacaoLoading").show();
    BloquearDiv("modalNovaCodificacao");
}

function OnSuccessCadastrarCodificacao(data) {
    $('#formCadastroCodificacao').removeAttr('style');
    $("#modalNovaCodificacaoLoading").hide();
    DesbloquearDiv("modalNovaCodificacao");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalNovaCodificacao').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnFailureCadastrarCodificacao() {
    $('#formCadastroCodificacao').removeAttr('style');
    $("#modalNovaCodificacaoLoading").hide();
    DesbloquearDiv("modalNovaCodificacao");

    ExibirMensagemDeErro("Erro ao tentar cadastrar a codificação do incidente. Tente novamente.");
}



function OnClickEditarCodificacao(origemElemento) {

    $('#modalEditarCodificacaoCorpo').html('');
    $('#modalEditarCodificacaoCorpoLoadingTexto').html('...Carregando formulário. Aguarde!');
    $('#modalEditarCodificacaoCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/Incidente/EditarCodificacao",
        data: { UKRelEnvolvido: origemElemento.data("uniquekeyrel"), Tipo: origemElemento.data("tipo"), UKCodificacao: origemElemento.data("ukcodificacao") },
        success: function (content) {
            $('#modalEditarCodificacaoCorpoLoading').hide();

            if (content.resultado != null && content.resultado != undefined) {

                if (content.resultado.Erro != null && content.resultado.Erro != undefined && content.resultado.Erro != "") {
                    var divErro = "" +
                        "<div class=\"alert alert-danger\">" +
                        "<strong>" +
                        "<i class=\"ace-icon fa fa-meh-o\"></i> " +
                        "Oops! " +
                        "</strong>" +

                        "<span>" + content.resultado.Erro + "</span>" +
                        "<br />" +
                        "</div>";

                    $('#modalEditarCodificacaoCorpo').html(divErro);

                }
            } else {

                $('#modalEditarCodificacaoCorpo').html(content);

                AplicaTooltip();

                Chosen();

                AplicaDatePicker();

                $("#modalEditarCodificacaoProsseguir").off("click").on('click', function (e) {
                    e.preventDefault();
                    $("#formEditarCodificacao").submit();
                });

            }
        }
    });
}

function OnBeginEditarCodificacao(jqXHR, settings) {
    $("#formEditarCodificacao").css({ opacity: "0.5" });
    $("#modalEditarCodificacaoLoading").show();
    BloquearDiv("modalEditarCodificacao");
}

function OnSuccessEditarCodificacao(data) {
    $('#formEditarCodificacao').removeAttr('style');
    $("#modalEditarCodificacaoLoading").hide();
    DesbloquearDiv("modalEditarCodificacao");

    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != undefined && data.resultado.Sucesso != "") {
        $('#modalEditarCodificacao').modal('hide');
        VisualizarDetalhesIncidente($('#modalDetalhesIncidenteCorpo'));
    }
}

function OnFailureEditarCodificacao() {
    $('#formEditarCodificacao').removeAttr('style');
    $("#modalEditarCodificacaoLoading").hide();
    DesbloquearDiv("modalEditarCodificacao");

    ExibirMensagemDeErro("Erro ao tentar cadastrar a codificação do incidente. Tente novamente.");
}

