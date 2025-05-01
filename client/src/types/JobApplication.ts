import { ApplicationStatus } from './ApplicationStatus';

export interface JobApplication {
	id: number;
	companyName: string;
	position: string;
	status: ApplicationStatus;
	dateApplied: string;
}
