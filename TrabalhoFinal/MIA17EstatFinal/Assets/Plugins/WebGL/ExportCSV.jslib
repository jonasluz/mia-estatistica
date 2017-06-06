mergeInto(LibraryManager.library, {

  /**
   * Exporta o arquivo CSV.
   * @param {string} csv Conteúdo CSV.
   * @param {int} dl valor 0 ou 1 de acordo com a intenção de se fazer o download do arquivo CSV. 
   */
  ExportCSV: function(csv, dl) {

    var FORMNAME  = 'csvfrm';
    var FIELDNAME = 'csvdata';
    var DLFIELD   = 'dl';
    var ACTIONURL = 'http://app.jonasluz.pro.br/Infection/UpdateCSV.php';
    var WINDOW    = 'spreadsheet';

    var form          = document.getElementById(FORMNAME); // recupera o formulário, se já existir.
    var contentField  = undefined;
    var dlField       = undefined;
    
    if (!form) {  // formulário inexistente; criar.
      form = document.createElement("form");
      form.setAttribute("id", FORMNAME);
      form.setAttribute("method", "post");
      form.setAttribute("action", ACTIONURL);
      form.setAttribute("target", WINDOW);
      // criar o campo do conteúdo CSV.
      var contentField = document.createElement("input"); 
      contentField.setAttribute("type", "hidden");
      contentField.setAttribute("id", FIELDNAME);
      contentField.setAttribute("name", FIELDNAME);
      form.appendChild(contentField);
      // criar o campo de marcação de download.
      var dlField = document.createElement("input");
      dlField.setAttribute("type", "hidden");
      dlField.setAttribute("id", DLFIELD);
      dlField.setAttribute("name", DLFIELD);
      form.appendChild(dlField);
      // adiciona formulário ao documento HTML.
      document.body.appendChild(form);
    }
    // atualiza dados CSV no campo específico.
    if (!contentField) contentField = document.getElementById(FIELDNAME);
    contentField.setAttribute("value", Pointer_stringify(csv));
    // atualiza o campo de marcação de download.
    if (!dlField) dlField = document.getElementById(DLFIELD);
    dlField.setAttribute("value", dl);
    // abre a janela da resposta e aciona o formulário.
    window.open('', WINDOW);
    form.submit();
  },
});