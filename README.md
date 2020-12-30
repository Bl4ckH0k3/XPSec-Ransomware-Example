# Ransomware Educacional - XPSec Security
<p align="center">
<img src="https://i.imgur.com/lK1VlSD.jpg">
</p>
<h3>Este código foi criado APENAS para fins EDUCACIONAIS, distribuído GRATUITAMENTE e OPEN SOURCE, a XPSec Security NÃO se responsabiliza por má uso do mesmo.</h3>
<br><hr><br>
<p>Após a <b>confirmação de que o usuário está ciente da ultilização</b> do Ransomware através da chave "CRYPTOME", o mesmo inicia o processo de criptografia, primeiramente gerando uma senha de 10 caracteres aleatória e em seguida captura o endereço da pasta onde se encontra, após isto cria um array contendo o nome de todos os arquivos ".txt" da pasta onde o mesmo se localiza e endereços de subpastas.<p>
  <p align="center">
<img src="https://i.imgur.com/UnKlXAS.png" width="70%">
  </p>
<p>Logo após, faz uma verificação por todos os arquivos dentro da pasta, caso a extensão do arquivo esteja dentre as extensões válidas, o malware o criptografa ultilizando um algorítimo AES e altera a extensão do arquivo para .xpsec (Para que possa identificar quais arquivos foram criptografados), repetindo o processo para cada arquivo válido um após o outro.</p>
<p align="center">
<img src="https://i.imgur.com/1OsG4H4.png" width="70%">
</p>
<p>*Em seguida, verifica uma a uma as subpastas e repete o processo de tentativa de criptografia até completar toda a rota programada.</p>
<p align="center">
<img src="https://i.imgur.com/dKAMOaf.png" width="70%">
</p>
<p>*Ao completar, exibe uma mensagem informando o término da criptografia e exibe uma mensagem contendo a senha de descriptografia.</p>
<p align="center">
<img src="https://i.imgur.com/MHFZmq6.png" width="70%">
</p>
<p><b>Seja um profissional em Segurança Ofensiva! Estude entre os melhores!<br></b></p>
<p><b>Acesse: https://xpsecsecurity.com/</b></p>
