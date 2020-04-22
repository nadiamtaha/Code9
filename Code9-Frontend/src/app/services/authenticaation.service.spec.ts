import { TestBed } from '@angular/core/testing';

import { AuthenticaationService } from './authenticaation.service';

describe('AuthenticaationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AuthenticaationService = TestBed.get(AuthenticaationService);
    expect(service).toBeTruthy();
  });
});
