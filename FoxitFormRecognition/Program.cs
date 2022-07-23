using foxit.common;
using foxit.pdf;
using foxit.pdf.interform;

// initialize the Foxit PDF SDK library
string sn = "<value_from_gsdk_sn.txt>";
string key = "<value_from_gsdk_key.txt>";
ErrorCode error_code = Library.Initialize(sn, key);
if (error_code != ErrorCode.e_ErrSuccess)
{
    Library.Release();
    return;
}

// load our Sample.pdf file
PDFDoc doc = new PDFDoc("Sample.pdf");
error_code = doc.Load(null);
if (error_code != ErrorCode.e_ErrSuccess)
{
    Library.Release();
    return;
}

// create a form object that will contain automatically added fields
using (Form form = new Form(doc))
{
    if (form.GetFieldCount("") == 0)
    {
        // the progressive class is used for long running tasks like loading documents,
        // parsing pages and in this case, automatic field recognition

        // start the Form Recognition Engine
        Progressive pro = doc.StartRecognizeForm(null);
        Progressive.State state = Progressive.State.e_ToBeContinued;

        // keep looping while the engine is running
        while (state == Progressive.State.e_ToBeContinued)
        {
            state = pro.Continue();
        }
    }
}

// Save the modified PDF to a new file
string newPdf = "Sample_With_Fields.pdf";
doc.SaveAs(newPdf, (int)PDFDoc.SaveFlags.e_SaveFlagNoOriginal);

// good practice to release the library when everything is done
Library.Release();