mergeInto(LibraryManager.library, {
   OpenExtURL: function (url) {
     window.open(Pointer_stringify(url), "_blank");
   }
 });