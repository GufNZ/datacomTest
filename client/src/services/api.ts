import axios from 'axios';

import { ApplicationStatus, type JobApplication, SortKey } from '../types';

const API_URL = 'http://localhost:5251/api/applications';

interface PaginatedResponse<T> {
	data: T[];
	total: number;
	page: number;
	pages: number;
	limit: number;
}

interface GetApplicationsArgs {
	status?: ApplicationStatus;
	company?: string;
	position?: string;
	sortBy?: SortKey;
	sortDirection: 'asc' | 'desc';
	page?: number;
	limit?: number;
}

export const api = {
	getApplications: async ({
		status,
		company,
		position,
		sortBy = SortKey.DateApplied,
		sortDirection = 'desc',
		page = 1,
		limit = 10
	}: GetApplicationsArgs) => {
		const params = new URLSearchParams({
			...(status && { status: status }),
			...(company && { company: company }),
			...(position && { position: position }),
			...(sortBy && { sortBy: sortBy }),
			...(sortDirection && { sortDirection: sortDirection }),
			page: page.toString(),
			limit: limit.toString()
		});

		const response = await axios.get<PaginatedResponse<JobApplication>>(
			`${API_URL}?${params.toString()}`
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
	deleteApplication: async (id: number) => {
		await axios.delete(`${API_URL}/${id}`);
		return true;
	},
	updateApplication: async (application: JobApplication) => {
		const response = await axios.put<JobApplication>(`${API_URL}/${application.id}`, application);
		return response.data;
	}
};
