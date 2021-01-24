import { Address } from "./entities";
import { reverseGeocoding } from "./geocoding";
import { httpReader } from "./reader.http.browser";

export async function japaneseReverseGeocoding(
    baseUrl: string,
    latitude: number,
    longitude: number
): Promise<Address | null> {
    const reader = httpReader(baseUrl);
    return await reverseGeocoding(reader, latitude, longitude);
}
