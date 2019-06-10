var express = require('express');
var app = express();
var path = require('path');
var mime = require('mime');
var fs = require('fs');
var crypto = require('crypto');

app.get('/', (req, res) => {
    res.send("servidor escuchando en 4567");
});

app.get('/getjson', (req, res) => {
    var file = __dirname + '/download/assetsTree.json';

    var filename = path.basename(file);
    var mimetype = mime.lookup(file);

    res.setHeader('Content-disposition', 'attachment; filename=' + filename);
    res.setHeader('Content-type', mimetype);

    var filestream = fs.createReadStream(file);
    filestream.pipe(res);
});

app.get('/getfile', (req, res) => {

  var file = __dirname + '/download/' + req.query.file;
  var filename = path.basename(file);
  var mimetype = mime.lookup(file);

  res.setHeader('Content-disposition', 'attachment; filename=' + filename);
  res.setHeader('Content-type', mimetype);

  // Algorithm depends on availability of OpenSSL on platform
  // Another algorithms: 'sha1', 'md5', 'sha256', 'sha512' ...
  var algorithm = 'md5';
  var shasum = crypto.createHash(algorithm);

  // Updating shasum with file content
  var s = fs.ReadStream(file);
  s.on('data', function(data) {
    shasum.update(data);
  });

  // making digest
  s.on('end', function() {
    var hash = shasum.digest('hex');
    console.log(hash + '  ' + file);
  });

    var filestream = fs.createReadStream(file);    
    filestream.pipe(res);
});



app.use(express.json());
app.listen(4567);

