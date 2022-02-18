const fs = require('fs');
const plainAddPlaceholder = require('./node-signpdf/dist/helpers/index').plainAddPlaceholder;
const signer = require('./node-signpdf/dist/signpdf').default;

module.exports = (callback, arg) => {

    const obj = JSON.parse(arg);

    const p12Buffer = fs.readFileSync(obj.P12Path);

    let pdfBuffer= fs.readFileSync(obj.PdfPath);

    pdfBuffer = plainAddPlaceholder({
        pdfBuffer: pdfBuffer,
        reason: obj.Reason,
        location: obj.Location,
        name: obj.Name,
        contactInfo: obj.ContactInfo,
        signatureLength: obj.SignatureLength,
        rect: obj.Rect
    });

    pdfBuffer = signer.sign(pdfBuffer, p12Buffer, { passphrase: obj.Passphrase });

    fs.writeFileSync(obj.OutputPath, pdfBuffer);

    callback(null, `New Signed PDF created: ${obj.OutputPath}`);
}