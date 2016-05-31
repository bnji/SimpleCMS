var maxFileSize = 2 * 1024 * 1024;
//var image = null;
var fileCount = 0;

//function HandleUpload(idPrefix, e, onSuccess) {
//    console.log(e);
//    var files = e.files;
//    if (files.length > 0) {
//        var size = files[0].size;
//        //alert(size + " / " + maxFileSize);
//        if (size > maxFileSize) {
//            e.preventDefault();
//            alert("Fílan er ov stór. Vinaliga vel eina aðra ella minka um støddina á fíluni.");
//        }
//        else {
//            var ext = files[0].extension;
//            if (ext === ".png" || ext === ".jpg" || ext === ".gif") {
//                SetFileData(idPrefix, files[0]);
//                if (onSuccess) {
//                    onSuccess();
//                }
//            }
//            else {
//                e.preventDefault();
//                alert("Bert myndir eru loyvdar. Vinaliga vel eina aðra fílu.");
//            }
//        }
//    }
//}
function SetFileData(idPrefix, file) {
    var fileName = file ? file.name : "";
    var fileExtension = file ? file.extension : "";
    var fileUID = file ? file.uid : "";
    //alert(idPrefix);
    $(idPrefix + "FileName").val(fileName);
    $(idPrefix + "Extension").val(fileExtension);
    $(idPrefix + "UID").val(fileUID);
}
function onUpload(e) {
    console.log(e);
    HandleUpload("#Files_" + fileCount + "_", e, function () {
        fileCount++;
        //alert(fileCount);
    });
}
function onSuccess() {
    //alert("file1 success");
}
if (typeof onComplete !== 'function') {
    onComplete = function () {
        //...
    }
}
if (typeof HandleUpload !== 'function') {
    HandleUpload = function (idPrefix, e, onSuccess) {
        console.log(e);
        var files = e.files;
        if (files.length > 0) {
            var size = files[0].size;
            //alert(size + " / " + maxFileSize);
            if (size > maxFileSize) {
                e.preventDefault();
                alert("Fílan er ov stór. Vinaliga vel eina aðra ella minka um støddina á fíluni.");
            }
            else {
                var ext = files[0].extension;
                if (ext === ".png" || ext === ".jpg" || ext === ".gif") {
                    SetFileData(idPrefix, files[0]);
                    if (onSuccess) {
                        onSuccess();
                    }
                }
                else {
                    e.preventDefault();
                    alert("Bert myndir eru loyvdar. Vinaliga vel eina aðra fílu.");
                }
            }
        }
    }
}
function onSelect(e) {
    //alert("onSelect");
    console.log(e);
}
function onCancel(e) {
    //alert("onCancel");
    SetFileData("#Files_" + fileCount + "_");
}
function onRemove(e) {
    //alert("onCancel");
    console.log(e);
    fileCount--;
    //alert(fileCount);
}