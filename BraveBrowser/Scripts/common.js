var appCommonAdmin = (function () {
  var instantiated;

  /**
  * __constructor
  */
  function init() {
    return {

      toastError: function (message) {
        return toastError(message);
      },
      toastWarning: function (message) {
        return toastWarning(message);
      },
      toastSuccess: function (message) {
        return toastSuccess(message);
      }
    };
  }

  /**
  * Singleton declare instance
  */
  return {
    getInstance: function () {
      if (typeof instantiated == "undefined" || instantiated == null) {
        instantiated = init();
      }
      return instantiated;
    }
  };

  /**
   * @description Private function
   */
  function toastError(message) {    
    setTimeout(function () {
      toastr.options = {
        closeButton: true,
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 4000
        //positionClass: "toast-top-left"
      };
      toastr.error('Error', message);
    }, 0);
  }

  function toastWarning(message) {
    setTimeout(function () {
      toastr.options = {
        closeButton: true,
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 4000
        //positionClass: "toast-top-left"
      };
      toastr.warning('Warning', message);
    }, 0);
  }

  function toastSuccess(message) {
    setTimeout(function () {
      toastr.options = {
        closeButton: true,
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 4000
        //positionClass: "toast-top-left"
      };
      toastr.success('Success', message);
    }, 0);
  }

})(jQuery);

(function ($) {

  $.fn.ShowDialogLoading = function () {
    $("#loading").show();
  }

  $.fn.CloseDialogLoading = function () {
    $("#loading").hide();
  }

}(jQuery));

