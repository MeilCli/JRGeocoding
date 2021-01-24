import { reverseGeocoding } from "./geocoding";
import { localReader } from "./reader.local";
import { httpReader } from "./reader.http.node";

if (process.argv.length < 4) {
    throw Error("Args must be latitude and longitude");
}
const latitude = parseFloat(process.argv[2]);
const longitude = parseFloat(process.argv[3]);
let isLocal = false;
if (5 <= process.argv.length && process.argv[4] == "--local") {
    isLocal = true;
}

async function run() {
    const reader = isLocal ? localReader("./public/data") : httpReader("https://jrgeocoding.meilcli.net/data");
    const address = await reverseGeocoding(reader, latitude, longitude);

    if (address != null) {
        console.log(`${address.prefecture}${address.city ?? ""}${address.ward ?? ""}${address.town ?? ""}`);
    } else {
        console.log("not found address");
    }
}

run();
