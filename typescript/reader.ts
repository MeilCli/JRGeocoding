import { Address } from "./entities";

export interface Reader {
    read: (fileName: string) => Promise<Address[] | null>;
}
