using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace System
{
    public class ModalDialogButton
    {
        public string ButtonTextValue { get; set; }
        public string ButtonClass { get; set; }
    }
    public static class BootstrapExtensions
    {
        public static MvcHtmlString ActionModalButton(this HtmlHelper helper, string dataTarget, string value, string btnClass, string btnId = "")
        {
            //dataTarget = "modalCRUD";
            //return MvcHtmlString.Create(string.Format("<button id='{3}' class='{2}' data-toggle='modal' data-target='#{0}'>{1}</button>", dataTarget, value, btnClass, btnId));
            return MvcHtmlString.Create(string.Format("<button class='{2}' data-toggle='modal' data-target='#{0}'>{1}</button>", dataTarget, value, btnClass));
        }

        public static MvcHtmlString ActionModalButton2(this HtmlHelper helper, string controllerName, string actionName, string modelName, string value, object htmlAttributes)
        {
            return MvcHtmlString.Create(string.Format("<button {4} data-toggle='modal' data-controller-name='{0}' data-action-name='{1}' data-model-name='{2}'>{3}</button>", controllerName, actionName, modelName, value, htmlAttributes.GetHtmlAttributesAsString()));
        }
        public static MvcHtmlString ActionModalButton2(this HtmlHelper helper, string dataTarget, string value, object htmlAttributes)
        {
            return MvcHtmlString.Create(string.Format("<button {2} data-toggle='modal' data-target='#{0}'>{1}</button>", dataTarget, value, htmlAttributes.GetHtmlAttributesAsString()));
        }

        public static MvcHtmlString ModalDialog(this HtmlHelper helper, string modalId, string modalCloseText, string modalSaveText, string modalTitle, string url, MvcHtmlString modalBody, object saveBtnHtmlAttributes, object closeBtnHtmlAttributes)
        {
            var createId = "create_" + modalId;
            var closeId = "close_" + modalId;
            var saveBtn = modalSaveText.Trim().Length > 0 ? "<button type='button' " + saveBtnHtmlAttributes.GetHtmlAttributesAsString() + " id='" + createId + "'>" + modalSaveText + "</button>" : "";
            var closeBtn = "";
            if(modalCloseText.Trim().Length > 0)
            {
                closeBtn = "<button type='button' " + closeBtnHtmlAttributes.GetHtmlAttributesAsString() + " id='" + closeId + "'>" + modalCloseText + "</button>";
            }
            else {
                throw new Exception("Missing 'Close Button' on Bootstrap Modal!");
            }
            var modal = @"<div class='modal fade' id='{0}' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>
                            <div class='modal-dialog'>
                                <div class='modal-content'>
                                    <div class='modal-header'>
                                        <!--<button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>{1}</span></button>-->
                                        <h4 class='modal-title'>{3}</h4>
                                    </div>
                                    <div class='modal-body'>
                                        {4}
                                    </div>
                                    <div class='modal-footer'>
                                        {2}
                                        {6}
                                    </div>
                                </div>
                                <!-- /.modal-content -->
                            </div>
                            <!-- /.modal-dialog -->
                        </div>
                        <!-- /.modal -->";
            return MvcHtmlString.Create(string.Format(modal, modalId, modalCloseText, saveBtn, modalTitle, modalBody, createId, closeBtn)).Concatenate(ModalDialogScript(helper, modalId, createId, closeId, url));
        }

        private static MvcHtmlString ModalDialogScript(this HtmlHelper helper, string modalId, string createId, string closeId, string url)
        {
            return MvcHtmlString.Create(@"<script>
                                            $(function () {
                                                $('#" + modalId + @"').find('form').submit(function (e) {
//alert('hey');
                                                    e.preventDefault();
                                                    //alert('" + url + @"');
                                                    //console.log($(this).serialize());
                                                    try {
                                                        $.post('" + url + @"', $(this).serialize(), function (d, s, xhr) {
                                                            //console.log(d); console.log(s); console.log(xhr);
                                                            if (d.length !== 0) {
                                                            //if(s === 'success') {
                                                                //console.log('OK');
                                                                $('#" + modalId + @"').modal('hide');

                                                            }
                                                        })
                                                        .fail(function(error) {
                                                            console.log(error);
                                                        });
                                                    } catch(errorPost) {
//                                                        try {
//                                                            $.get('" + url + @"', $(this).serialize(), function (data) {
//                                                                //console.log(d); console.log(s); console.log(xhr);
//                                                                $('#" + modalId + @"').modal('hide');
//                                                            });
//                                                        }
//                                                        catch(errorGet) { }
                                                    }
                                                });
                                                $('#" + createId + @"').click(function (e) {
console.log('form submit...');
console.log(e);
//alert('saving...');
                                                    $('#" + modalId + @"').find('form').submit();
                                                    //$('form').submit();
                                                });
                                                //$('#" + closeId + @"').click(function () {
                                                $('.close-modal').click(function () {
                                                    canLoadList = false;
                                                    $('.modal').modal('hide');
                                                });
                                                $('.modal').modal({
                                                    backdrop : false,
                                                    keyboard : true,
                                                    show : false
                                                });
                                            });
                                        </script>");
        }

        //@Html.ListView("createWebsiteModal", "/Person/WebsiteList", new { id: "@Model.Id" })
        [Obsolete("Instead use the loadList JavaScript function")]
        public static MvcHtmlString ListView(this HtmlHelper helper, string modalId, string url, int? id)
        {
            var divId = "list_" + new Random((int)DateTime.Now.Ticks).Next(0, int.MaxValue);//.AppendRandomNumberString();DateTime.Now.Ticks;
            var data = id != null ? "{ id: '" + id + @"' }" : "{ }";
            return MvcHtmlString.Create(@"<div id='" + divId + @"'></div>
            <script>
                var canLoadList = true;
                $(function () {
                    // Don't load list on ESCAPE key
                    $('.modal').keydown(function(e){
                        canLoadList = e.keyCode !== 27; // ESC-key
                        //console.log('canLoadList: ' + canLoadList + ', keyCode: ' + e.keyCode);
                    });
                    loadList();
                });
                function loadList() {
                    var $detailDiv = $('#" + divId + @"');
                    $detailDiv.html('<p class=\'text-center\' style=\'padding-top:25px;\'><img src=\'/Content/images/ajax-loader.gif\' /></p>');
                    //alert('OK');
                    $.get('" + url + @"', " + data + @", function (data) {
                        $detailDiv.html(data);
                        $('.modal').on('hidden.bs.modal', function (e) {
                            //console.log(e);
                            if(canLoadList) {
                                loadList();
                            }
                            // Make sure we can load the list again
                            canLoadList = true;
                            $('.modal').die();
                        });
                    })
                    .fail(function(error) {
                        var errorString = '<h3 style=\'color: red;\'>Modal Id: " + modalId + @"</h3<br />';
                        errorString += '<h3 style=\'color: red;\'>Url: " + url + @"</h3><br />';
                        errorString += error['responseText'];
                        $('#modalDebug .modal-body').html(errorString);
                        $('#modalDebug').modal({
                            keyboard: false
                        }).modal('show');
//                        $('body').html('');
                        console.log('modalId: " + modalId + @"');
                        console.log('url: " + url + @"');
                        console.log(error);
//                        alert('Failed on jQuery.get()\nPlease contact the system administrator for more help.\nSee browser console for more details.');
                    });
                }
            </script>");
            //@Html.ListView("websiteList", "createWebsiteModal", "/Person/WebsiteList", { id: "@Model.Id" })
        }

        public static MvcHtmlString Concatenate(this MvcHtmlString first, params MvcHtmlString[] strings)
        {
            return MvcHtmlString.Create(first.ToString() + string.Concat(strings.Select(s => s.ToString())));
        }
    }
}
