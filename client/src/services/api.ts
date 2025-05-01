import axios from 'axios';

import { JobApplication } from '../types/JobApplication';

const API_URL = 'http://localhost:5251/api/applications';

interface PaginatedResponse<T> {
	data: T[];
	total: number;
	page: number;
	pages: number;
	limit: number;
}

export const api = {
	getApplications: async (page: number = 1, limit: number = 10) => {
		const response = await axios.get<PaginatedResponse<JobApplication>>(
			`${API_URL}?page=${page}&limit=${limit}`
		);
		return response.data;
	},
	getApplication: async (id: number) => {
		const response = await axios.get<JobApplication>(`${API_URL}/${id}`);
		return response.data;
	},
	createApplication: async (application: Omit<JobApplication, 'id'>) => {
		const response = await axios.post<JobApplication>(API_URL, application);
		return response.data;
	},
	updateApplication: async (application: JobApplication) => {
		const response = await axios.put<JobApplication>(`${API_URL}/${application.id}`, application);
		return response.data;
	}
};
