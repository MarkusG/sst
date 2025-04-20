import axios, {AxiosInstance} from "axios";

let client: AxiosInstance | null = null;

export default function useAxios() {
    if (client)
        return client;

    client = axios.create({
        baseURL: 'https://localhost:5001/'
    });

    return client;
}