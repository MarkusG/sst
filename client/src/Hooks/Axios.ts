import axios, {AxiosInstance} from "axios";

export const SubmitFormOptions = {
    headers: {'Content-Type': 'multipart/form-data'},
    formSerializer: {
        indexes: null
    }
};

let client: AxiosInstance | null = null;

export default function useAxios() {
    if (client)
        return client;

    client = axios.create({
        baseURL: 'https://localhost:5001/'
    });

    return client;
}