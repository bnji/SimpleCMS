 $(function () {
    shortcut.add("Ctrl+O", function (e) {
        e.preventDefault();
        alert("Hi there!");
    });
    shortcut.add("Ctrl+.", function (e) {
        e.preventDefault();
        alert("Hi theresdf!"); 
    });
 });