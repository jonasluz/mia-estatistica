<!DOCTYPE html>
<?php
/**
 * UpdateCSV.php 
 * Atualiza o arquivo CSV no servidor, a ser utilizado pela Planilha Google Docs.
 *
 */

const FILENAME = 'log.csv';
const SPREADSHEET = 'https://docs.google.com/spreadsheets/d/18kBzjpnI5E7vo8nh0hLZxmhT93UZSpxZYfD32djLLiM';

$msg = 'Gerando arquivo CSV... Por favor, espere.';

// Direcionar à planilha ou download do CSV?
$target = $_POST['dl'] == '1' ? FILENAME : SPREADSHEET;

// Lê conteúdo enviado.
$content = $_POST['csvdata'];
if (!$content || $content == "") $msg = "Conteudo CSV enviado vazio: '" . $content . "'";
else {
    // Grava conteúdo enviado no arquivo.
    $result = file_put_contents(FILENAME, $content);
    if ($result === false) $msg = "Nao consegui salvar o arquivo " . FILENAME;
}
?>
<html lang="pt_BR">
    <head>
        <title><?php echo $msg; ?></title>
        <meta charset="UTF-8">
        <script>
            var redirect = () => {
                window.location.replace('<?php echo $target; ?>')
            }
        </script>
    </head>
    <body onLoad="window.setTimeout(redirect, 1000);">
        <div id="message"><?php echo $msg; ?></div>
        <img id="loadingIcon" src="Flask.svg" alt="Carregando..."/>
    </body>
</html>