import fetch from "node-fetch";
import { Reader } from "./reader";
import { Address } from "./entities";

export function httpReader(baseUrl: string): Reader {
    let baseUri = baseUrl;
    if (baseUrl.endsWith("/") == false) {
        baseUri = `${baseUrl}/`;
    }
    return {
        read: async (fileName: string) => {
            const result = await fetch(baseUri + fileName);
            const json = await result.json();
            return json as Address[];
        },
    };
}
