import * as fs from "fs";
import * as path from "path";
import { Reader } from "./reader";
import { Address } from "./entities";

export function localReader(dataFolder: string): Reader {
    return {
        read: async (fileName: string) => {
            const content = fs.readFileSync(path.join(dataFolder, fileName));
            const json = content.toString();
            return JSON.parse(json) as Address[];
        },
    };
}
