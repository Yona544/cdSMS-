<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="css/jquery.tag-editor.css">
</head>
<body>
    <form id="form1" action="uploadmp3.aspx" method="post" enctype="multipart/form-data">
        <div>
            <table class="gridtable" id="Table2" cellspacing="1" cellpadding="4" width="50%"
                border="0">
                <tbody>


                    <tr class="tblrow">
                        <td valign="middle" align="left" width="20%" class="tblrowheader">MP3 File
                                                </td>
                        <td valign="middle" align="left" class="tblrow">
                            <input type="file" name="file" id="file" />
                            <input type="hidden" name="hdAdmin" id="hdAdmin" value="Admin" />
                        </td>
                    </tr>
                    <tr class="tblrow">
                        <td valign="middle" align="left" width="20%" class="tblrowheader">Description
                                                </td>
                        <td valign="middle" align="left" class="tblrow">
                            <textarea id="description" name="description" rows="4" cols="70"></textarea>
                        </td>
                    </tr>
                    <tr class="tblrow">
                        <td valign="middle" align="left" width="20%" class="tblrowheader">Caller Number
                                                </td>
                        <td valign="middle" align="left" class="tblrow">
                            <input type="text" id="txtCaller" name="txtCaller" />
                        </td>
                    </tr>
                    <tr class="tblrow">
                        <td valign="middle" align="left" width="20%" class="tblrowheader">Tags
                                                </td>
                        <td valign="middle" align="left" class="tblrow">
                            <textarea id="mytags" name="mytags" rows="4"></textarea>
                        </td>
                    </tr>

                    <tr class="tblrow">
                        <td colspan="2" align="center">
                            <input type="submit" name="submit" value="Submit" />
                        </td>

                    </tr>

                </tbody>
            </table>




        </div>
        <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
        <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.min.js"></script>
        <script src="js/jquery.caret.min.js"></script>
        <script src="js/jquery.tag-editor.js"></script>
        <script>
            $(function () {
                $('#mytags').tagEditor({
                    delimiter: ', ', /* space and comma */
                    placeholder: 'Enter tags ...'
                });
            })
            </script>
    </form>
</body>
</html>
