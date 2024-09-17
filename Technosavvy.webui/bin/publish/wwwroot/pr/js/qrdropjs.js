
      var inputCopyGroups = document.querySelectorAll('.input-group-copy');
   
   for (var i = 0; i < inputCopyGroups.length; i++) {
     var _this = inputCopyGroups[i];
     var btn = _this.getElementsByClassName('btn')[0];
     var input = _this.getElementsByClassName('form-control')[0];
   
     input.addEventListener('click', function(e) {
       this.select();
     });
     btn.addEventListener('click', function(e) {
       var input = this.parentNode.parentNode.getElementsByClassName('form-control')[0];
       input.select();
       try {
         var success = document.execCommand('copy');
         console.log('Copying ' + (success ? 'Succeeded' : 'Failed'));
       } catch (err) {
         console.log('Copying failed');
       }
     });
   }
   