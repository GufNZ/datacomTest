import { ApplicationStatus, SortKey } from '../../types';

export interface FilterState {
	status: ApplicationStatus | undefined;
	company: string;
	position: string;
}

export interface TextFilterState {
	company: string;
	position: string;
}

export interface SortConfig {
	key: SortKey;
	direction: 'asc' | 'desc';
}

export interface TableColumn {
	id: string;
	label: string;
	align: 'left' | 'right' | 'center';
	sortable?: boolean;
}
