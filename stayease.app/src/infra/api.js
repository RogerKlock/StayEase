import axios from "axios";

const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL || "/api",
    headers: {
        "Content-Type": "application/json",
    },
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
        const data = error?.response?.data;
        const message =
            data?.detail ||
            data?.message ||
            data?.title ||
            error?.message ||
            "Erro ao comunicar com a API.";

        return Promise.reject(new Error(message));
    }
);

export default api;
